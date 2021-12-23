// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       24.11.2021 09:56
// Developer:     Matthias Mandl
// Project:       WebAPI
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Biss.Serialize;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;

namespace WebAPI.MqttCommunication
{
    /// <summary>
    ///     Handler a mqtt connection
    /// </summary>
    public class MqttConnectionHandlerServer
    {
        /// <summary>
        ///     The mqtt client
        /// </summary>
        private static IMqttClient _mqttClient;

        /// <summary>
        ///     Pairs of a unique ID of a device and the related ID from the project
        /// </summary>
        public static readonly Dictionary<string, string> DeviceProjectIdPairs = new Dictionary<string, string>(); // device id(eg windows UUID) and virtual id from Project (eg 21) 

        /// <summary>
        ///     The uids of the devices
        /// </summary>
        public static readonly List<string> DeviceUids = new List<string>();

        #region Properties

        /// <summary>
        ///     The mqtt client
        /// </summary>
        private static IMqttClient MqttClient
        {
            get => _mqttClient;
            set => _mqttClient = value ?? throw new ArgumentNullException(nameof(MqttClient));
        }

        #endregion

        /// <summary>
        ///     Start steps to get the wright id for this device
        /// </summary>
        /// <returns>A Task</returns>
        public async Task StartAsync()
        {
            MqttClient = (new MqttFactory()).CreateMqttClient();

            var localIpAddress = Helper.GetLocalIpAddress().ToString();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer(localIpAddress, 1883)
                .Build();

            await MqttClient.ConnectAsync(options).ConfigureAwait(true);

            MqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
            {
                switch (e.ApplicationMessage.Topic)
                {
                    case "SendUID":
                        Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        break;
                }
            });
        }

        /// <summary>
        ///     Sends the project as json, to see all changes of all gateways
        /// </summary>
        /// <param name="project">The project</param>
        /// <returns>A task</returns>
        public async Task SendGatewaysChangesAsync(Project project)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("GatewaysChanges")
                .WithPayload(project.ToJson())
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttClient.PublishAsync(message).ConfigureAwait(true);
        }
    }
}