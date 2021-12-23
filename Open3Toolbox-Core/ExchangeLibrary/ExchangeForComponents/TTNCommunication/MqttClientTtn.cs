// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Biss.Serialize;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using Newtonsoft.Json.Linq;

namespace ExchangeLibrary.TTNCommunication
{
    /// <summary>
    ///     An mqtt client for comunicating over ttn
    /// </summary>
    public class MqttClientTtn
    {
        /// <summary>
        ///     Will be executed, if an uplink message is received
        /// </summary>
        private readonly ICommand _uplinkMessageReceivedCommand;

        /// <summary>
        ///     The mqtt client
        /// </summary>
        private IMqttClient? _mqttClient;

        /// <summary>
        ///     Initializes a mqtt client for comunicating over ttn
        /// </summary>
        /// <param name="uplinkMessageReceivedCommand">Will be executed, if an uplink message is received</param>
        public MqttClientTtn(ICommand uplinkMessageReceivedCommand)
        {
            _uplinkMessageReceivedCommand = uplinkMessageReceivedCommand ?? throw new ArgumentNullException(nameof(uplinkMessageReceivedCommand));
        }

        #region Properties

        /// <summary>
        ///     The mqtt client
        /// </summary>
        private IMqttClient MqttClient
        {
            get => _mqttClient ?? throw new ArgumentNullException(nameof(MqttClient));
            set => _mqttClient = value ?? throw new ArgumentNullException(nameof(MqttClient));
        }

        #endregion

        /// <summary>
        ///     Starts a mqtt client and the communication async
        /// </summary>
        /// <returns>A Task</returns>
        public async Task StartAsync()
        {
            MqttClient = (new MqttFactory()).CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("eu1.cloud.thethings.network", 1883).WithCredentials(
                    "test-application-dataskop@ttn", "TODO") 
                .Build();

            await MqttClient.ConnectAsync(options).ConfigureAwait(true);

            MqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
            {
                Console.WriteLine("Device" + e.ClientId);
                Console.WriteLine("Topic:" + e.ApplicationMessage.Topic);

                var obj = BissDeserialize.FromJson<object>(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                var jsonObj = JObject.Parse(Encoding.UTF8.GetString(e.ApplicationMessage.Payload));
                var correlation_ids = jsonObj.GetValue("correlation_ids", StringComparison.CurrentCulture);
                var endDeviceIds = jsonObj.GetValue("end_device_ids", StringComparison.CurrentCulture);
                var deviceId = endDeviceIds!.Value<string>("device_id"); // device ID of device

                Console.WriteLine("Got something from DEVICE: " + deviceId + /*" CORRELATIONIDS: " + correlation_ids +*/ " TOPIC: " + e.ApplicationMessage.Topic);

                switch (e.ApplicationMessage.Topic)
                {
                    case "v3/test-application-dataskop@ttn/devices/eui-70b3d57ed0044ad7/up":
                        var uplinkMessage = jsonObj.GetValue("uplink_message", StringComparison.CurrentCulture);
                        var timestamp = jsonObj.GetValue("received_at", StringComparison.CurrentCulture);
                        var upPort = uplinkMessage!.Value<string>("f_port");
                        var upCnt = uplinkMessage.Value<string>("f_cnt");
                        var upFrmPayload = uplinkMessage.Value<string>("frm_payload");

                        var jObjectUpDecodedPayload = uplinkMessage.Value<JObject>("decoded_payload");
                        var upDecodedPayload = jObjectUpDecodedPayload!.ToString(); // PAYLOAD

                        var jObjectUpRxMetadata = uplinkMessage.Value<JArray>("rx_metadata");
                        var upRxMetadata = jObjectUpRxMetadata!.ToString();

                        var jObjectupSettings = uplinkMessage.Value<JObject>("settings");
                        var upSettings = jObjectupSettings!.ToString();

                        var upReceivedAt = uplinkMessage.Value<string>("received_at");
                        var upConsumedAirtime = uplinkMessage.Value<string>("condumed_airtime");

                        Console.WriteLine("I got Uplink message PACKET RECEIVED AT: " + timestamp + " PORT: " + upPort + " CNT: " + upCnt + " FRMPAYLOAD: " + upFrmPayload + " DECODEDPAYLOAD: " + upDecodedPayload);

                        _uplinkMessageReceivedCommand.Execute(new Tuple<string, string>(deviceId!, upDecodedPayload));


                        break;
                    case "v3/test-application-dataskop@ttn/devices/eui-70b3d57ed0044ad7/down/queued":
                        var downlinkQueued = jsonObj.GetValue("downlink_queued", StringComparison.CurrentCulture);
                        var downPort = downlinkQueued!.Value<string>("f_port");
                        var downFrmPayload = downlinkQueued.Value<string>("frm_payload");
                        var downPriority = downlinkQueued.Value<string>("priority");
                        Console.WriteLine("I got Downlink queued message PORT: " + downPort + " PAYLOAD: " + downFrmPayload + " PRIORITY: " + downPriority);
                        break;
                }
            });
            await GetAll().ConfigureAwait(true);
        }

        /// <summary>
        ///     Getting all
        /// </summary>
        /// <returns>A Task</returns>
        public async Task GetAll()
        {
            var options = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(new MqttTopicFilterBuilder().WithTopic("v3/test-application-dataskop@ttn/devices/eui-70b3d57ed0044ad7/#"))
                .Build();

            await MqttClient.SubscribeAsync(options).ConfigureAwait(true);
        }

        /// <summary>
        ///     Sends a downlink message every xxx to the sensor
        /// </summary>
        /// <param name="shouldRun">If the loop should run</param>
        /// <returns>A Task</returns>
        public async Task SendDownlinkWorker(bool shouldRun)
        {
            while (!shouldRun)
            {
                await SendDownlinkMessage("{\"downlinks\": [{\"f_port\": 15,\"frm_payload\": \"MATT\", \"priority\": \"NORMAL\"}]}").ConfigureAwait(true);
                Thread.Sleep(20000);
            }
        }

        /// <summary>
        ///     Sends a downlink message to the sensor
        /// </summary>
        /// <param name="payload">The downlink message</param>
        /// <returns>A Task</returns>
        public async Task SendDownlinkMessage(string payload)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("v3/test-application-dataskop@ttn/devices/eui-70b3d57ed0044ad7/down/push")
                .WithPayload(payload)
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await MqttClient.PublishAsync(message).ConfigureAwait(true);
        }
    }
}