// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       24.11.2021 09:44
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Biss.Serialize;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;

namespace ExchangeLibrary.ExchangeForComponents.MqttCommunication
{
    /// <summary>
    ///     Represents a mqtt connection handler
    /// </summary>
    public class MqttConnectionHandlerClient
    {
        /// <summary>
        ///     the unique id of the device
        /// </summary>
        private readonly string _uid;

        /// <summary>
        ///     The Uri for server
        /// </summary>
        private readonly Uri _uri;

        /// <summary>
        ///     The id of the client
        /// </summary>
        private long _id;

        /// <summary>
        ///     The command, if the id is received from the server
        /// </summary>
        private ICommand? _idReceivedCommand;

        /// <summary>
        ///     The mqtt client
        /// </summary>
        private IMqttClient? _mqttClient;

        /// <summary>
        ///     Initializes a mqtt conenction handler
        /// </summary>
        /// <param name="serverUri">The uri of the server to connect</param>
        /// <param name="uid">the unique id of the device</param>
        public MqttConnectionHandlerClient(Uri serverUri, string uid)
        {
            _uid = uid;
            _uri = serverUri ?? throw new ArgumentNullException(nameof(serverUri));
        }

        #region Properties

        /// <summary>
        ///     The id of the client
        /// </summary>
        private long Id
        {
            get => _id;
            set => _id = value;
        }

        /// <summary>
        ///     The mqtt client
        /// </summary>
        private IMqttClient MqttClient
        {
            get => _mqttClient ?? throw new ArgumentNullException(nameof(MqttClient));
            set => _mqttClient = value ?? throw new ArgumentNullException(nameof(MqttClient));
        }

        /// <summary>
        ///     The command, if the id is received from the server
        /// </summary>
        private ICommand IdReceivedCommand
        {
            get => _idReceivedCommand ?? throw new ArgumentNullException(nameof(IdReceivedCommand));
            set => _idReceivedCommand = value ?? throw new ArgumentNullException(nameof(IdReceivedCommand));
        }

        #endregion

        /// <summary>
        ///     Start steps to get the wright id for this device
        /// </summary>
        /// <param name="receivedCommand">The command which will get executed, when an id arrives</param>
        /// <returns>A Task</returns>
        public async Task StartAsync(ICommand receivedCommand)
        {
            IdReceivedCommand = receivedCommand ?? throw new ArgumentNullException(nameof(receivedCommand));
            MqttClient = (new MqttFactory()).CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(_uri.Host, 1883)
                .Build();

            await MqttClient.ConnectAsync(options).ConfigureAwait(true);

            MqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
            {
                switch (e.ApplicationMessage.Topic)
                {
                    case "GatewaysChanges":
                        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        BissDeserialize.FromJson<Dictionary<string, string>>(payload);
                        break;
                }
            });
            await GatewaysChanges().ConfigureAwait(true);
        }

        /// <summary>
        ///     Subscribes to the gatewaychanges topic, so it recognizes
        ///     if any gateway has changed
        /// </summary>
        /// <returns>A task</returns>
        public async Task GatewaysChanges()
        {
            var options = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(new MqttTopicFilterBuilder().WithTopic("GatewaysChanges"))
                .Build();

            await MqttClient.SubscribeAsync(options).ConfigureAwait(true);
        }
    }
}