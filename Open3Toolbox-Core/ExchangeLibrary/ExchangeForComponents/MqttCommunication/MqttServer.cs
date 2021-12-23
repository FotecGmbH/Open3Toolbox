// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       24.11.2021 09:41
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;

namespace ExchangeLibrary.ExchangeForComponents.MqttCommunication
{
    /// <summary>
    ///     Represents a mqtt server
    /// </summary>
    public static class MqttServer
    {
        /// <summary>
        ///     The main method that starts the service.
        /// </summary>
        public static async Task StartAsync()
        {
            var optionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpoint().WithDefaultEndpointPort(1883)
                .WithSubscriptionInterceptor(
                    c =>
                    {
                        c.AcceptSubscription = true;
                        LogMessage(c, true);
                    }).WithApplicationMessageInterceptor(
                    c =>
                    {
                        c.AcceptPublish = true;
                        LogMessage(c);
                    });

            var mqttServer = new MqttFactory().CreateMqttServer();
            await mqttServer.StartAsync(optionsBuilder.Build()).ConfigureAwait(true);
        }


        /// <summary>
        ///     Logs the message from the MQTT subscription interceptor context.
        /// </summary>
        /// <param name="context">The MQTT subscription interceptor context.</param>
        /// <param name="successful">A <see cref="bool" /> value indicating whether the subscription was successful or not.</param>
        private static void LogMessage(MqttSubscriptionInterceptorContext context, bool successful)
        {
            Console.WriteLine(
                successful
                    ? "New subscription: ClientId =" + context.ClientId + "TopicFilter= " + context.TopicFilter.Topic
                    : "Subscription failed for clientId = +context.ClientId" + "TopicFilter= " + context.TopicFilter.Topic);
        }

        /// <summary>
        ///     Logs the message from the MQTT message interceptor context.
        /// </summary>
        /// <param name="context">The MQTT message interceptor context.</param>
        private static void LogMessage(MqttApplicationMessageInterceptorContext context)
        {
            var payload = context.ApplicationMessage?.Payload == null ? null : Encoding.UTF8.GetString(context.ApplicationMessage?.Payload);

            Console.WriteLine(
                "Message: ClientId = {clientId}, Topic = {topic}, Payload = {payload}, QoS = {qos}, Retain-Flag = {retainFlag}",
                context.ClientId,
                context.ApplicationMessage?.Topic,
                payload,
                context.ApplicationMessage?.QualityOfServiceLevel,
                context.ApplicationMessage?.Retain);
        }

        /// <summary>
        ///     Logs the message from the MQTT connection validation context.
        /// </summary>
        /// <param name="context">The MQTT connection validation context.</param>
        /// <param name="showPassword">A <see cref="bool" /> value indicating whether the password is written to the log or not.</param>
        private static void LogMessage(MqttConnectionValidatorContext context, bool showPassword)
        {
            if (showPassword)
            {
                Console.WriteLine(
                    "New connection: ClientId = {clientId}, Endpoint = {endpoint}, Username = {userName}, Password = {password}, CleanSession = {cleanSession}",
                    context.ClientId,
                    context.Endpoint,
                    context.Username,
                    context.Password,
                    context.CleanSession);
            }
            else
            {
                Console.WriteLine(
                    "New connection: ClientId = {clientId}, Endpoint = {endpoint}, Username = {userName}, CleanSession = {cleanSession}",
                    context.ClientId,
                    context.Endpoint,
                    context.Username,
                    context.CleanSession);
            }
        }
    }
}