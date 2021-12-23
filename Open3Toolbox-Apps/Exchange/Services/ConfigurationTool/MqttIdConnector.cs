// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Biss.Serialize;
using Exchange.Services.ConfigurationTool.Interfaces;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;

namespace Exchange.Services.ConfigurationTool
{
    /// <summary>
    ///     Echte MQTT Connector, zur Zeit unwichtig.
    /// </summary>
    /// <seealso cref="Exchange.Services.ConfigurationTool.Interfaces.IIdConnector" />
    [Obsolete("Is currently Not in use")]
    public class MqttIdConnector : IIdConnector
    {
        /// <summary>
        ///     The exit
        /// </summary>
        private bool _exit;

        /// <summary>
        ///     The uids
        /// </summary>
        public List<string> _uids = new List<string>();

        /// <summary>
        ///     The MQTT client
        /// </summary>
        private IMqttClient mqttClient;

        /// <summary>
        ///     Sends the pairs asynchronous.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        public async Task SendPairsAsync(Dictionary<string, long> pairs)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("GetPairs")
                .WithPayload(pairs.ToJson())
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await mqttClient.PublishAsync(message).ConfigureAwait(true);
        }

        /// <summary>
        ///     MQTTs the configuration.
        /// </summary>
        private async Task MqttConfig()
        {
            mqttClient = (new MqttFactory()).CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.100.217", 1883) // hardcoded
                .Build();

            await mqttClient.ConnectAsync(options).ConfigureAwait(true);

            await GetUidss().ConfigureAwait(true);
            await SendGetUidsAsync().ConfigureAwait(true);
        }

        /// <summary>
        ///     Gets the uidss.
        /// </summary>
        private async Task GetUidss()
        {
            var options = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(new MqttTopicFilterBuilder().WithTopic("SendUIDs"))
                .Build();
            await mqttClient.SubscribeAsync(options).ConfigureAwait(true);

            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(async e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine("ff: " + e.ApplicationMessage.Topic + " - " + payload);

                List<string> devices = BissDeserialize.FromJson<List<string>>(payload); //  LIST WITH ALL DEVICES!!!
                _uids = devices;

                _exit = true;
            });
        }

        /// <summary>
        ///     Sends the get uids asynchronous.
        /// </summary>
        private async Task SendGetUidsAsync()
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("GetUIDs")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await mqttClient.PublishAsync(message).ConfigureAwait(true);
        }

        #region Interface Implementations

        /// <summary>
        ///     Gets the uids.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUids()
        {
            Task.Run(async () => await MqttConfig().ConfigureAwait(true));
            while (!_exit)
            {
                Thread.Sleep(200);
            }

            _exit = false;

            return _uids;
        }

        /// <summary>
        ///     Sends the pairs.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        public void SendPairs(Dictionary<string, long> pairs)
        {
            Task.Run(async () => await SendPairsAsync(pairs).ConfigureAwait(true));
        }

        #endregion
    }
}