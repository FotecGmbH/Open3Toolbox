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
using System.Json;
using System.Linq;
using Exchange.Model.ConfigurationTool;
using Exchange.Model.ConfigurationTool.Interfaces;
using ExConfigExchange.Services;

namespace Exchange.Services.ConfigurationTool
{
    /// <summary>
    ///     Konvertiert konfigurierbare Modelle in JSON, dass deren nicht konfigurierbaren Versionen "beschreibt".
    /// </summary>
    public static class ExConfigurableJsonConverter
    {
        /// <summary>
        ///     Konvertiert zu JSON, der <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Project" /> "beschreibt",
        ///     ohne Gateways und Sensoren.
        /// </summary>
        /// <param name="id">Der Id von der echten Modell.</param>
        /// <param name="configurable">Der konfigurierbaren Modell.</param>
        /// <returns>JSON <see cref="string" /> für <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Project" />.</returns>
        public static string ToJSONNoChildren(long id, ExProject configurable)
        {
            return CreateExConfigurableJsonObject(id, configurable).ToString();
        }

        /// <summary>
        ///     Konvertiert zu JSON, der <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Gateway" /> "beschreibt",
        ///     ohne Sensoren.
        /// </summary>
        /// <param name="id">Der Id von der echten Modell.</param>
        /// <param name="configurable">Der konfigurierbaren Modell.</param>
        /// <returns>JSON <see cref="string" /> für <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Gateway" />.</returns>
        public static string ToJSONNoChildren(long id, ExGateway configurable)
        {
            return CreateExConfigurableJsonObject(id, configurable).ToString();
        }

        /// <summary>
        ///     Konvertiert zu JSON, der <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Sensor" /> "beschreibt".
        /// </summary>
        /// <param name="id">Der Id von der echten Modell.</param>
        /// <param name="configurable">Der konfigurierbaren Modell.</param>
        /// <returns>JSON <see cref="string" /> für <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Sensor" />.</returns>
        public static string ToJSON(long id, ExSensor configurable)
        {
            return CreateExConfigurableJsonObject(id, configurable).ToString();
        }

        /// <summary>
        ///     Konvertiert zu JSON, der <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Project" /> "beschreibt",
        ///     mit Gateways und Sensoren.
        /// </summary>
        /// <param name="id">Der Id von der echten Modell.</param>
        /// <param name="configurable">Der konfigurierbaren Modell.</param>
        /// <param name="gateways">Die Gateways vom Projekt.</param>
        /// <param name="sensors">Die Sensoren vom Projekt.</param>
        /// <returns>JSON <see cref="string" /> für <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Project" />.</returns>
        public static string ToJSON(long id, ExProject configurable, List<Tuple<long, ExGateway>> gateways, List<Tuple<long, ExSensor>> sensors)
        {
            var root = CreateExConfigurableJsonObject(id, configurable);

            var gatewaysJson = new JsonArray();
            root.Add("gateways", gatewaysJson);

            foreach (var gateway in gateways)
            {
                gatewaysJson.Add(JsonValue.Parse(ToJSON(gateway.Item1, gateway.Item2, sensors)));
            }

            var json = root.ToString();

            return json;
        }

        /// <summary>
        ///     Konvertiert zu JSON, der <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Project" /> "beschreibt",
        ///     mit Sensoren.
        /// </summary>
        /// <param name="id">Der Id von der echten Modell.</param>
        /// <param name="configurable">Der konfigurierbaren Modell.</param>
        /// <param name="sensors">Die Sensoren vom Gateway.</param>
        /// <returns>JSON <see cref="string" /> für <see cref="ExchangeLibrary.SensorData.GeneratedModels.Generated.Gateway" />.</returns>
        public static string ToJSON(long id, ExGateway configurable, List<Tuple<long, ExSensor>> sensors)
        {
            var root = CreateExConfigurableJsonObject(id, configurable);

            var sensorsJson = new JsonArray();
            root.Add("sensors", sensorsJson);

            foreach (var sensor in sensors.Where(s => s.Item2.GatewayId == id))
            {
                sensorsJson.Add(JsonValue.Parse(ToJSON(sensor.Item1, sensor.Item2)));
            }

            return root.ToString();
        }

        /// <summary>
        ///     Konvertiert die geteilte Properties von <see cref="IExConfigurable" /> zu einen <see cref="JsonObject" />.
        /// </summary>
        /// <param name="id">Der Id von der echten Modell.</param>
        /// <param name="configurable">Der konfigurierbaren Modell.</param>
        /// <returns><see cref="JsonObject" />, der den echten Modell beschreibt.</returns>
        private static JsonObject CreateExConfigurableJsonObject(long id, IExConfigurable configurable)
        {
            var root = new JsonObject();

            root.Add("id", JsonValue.Parse(id.ToString()));
            root.Add("name", JsonValue.Parse($"\"{configurable.Name}\""));
            root.Add("description", JsonValue.Parse($"\"{configurable.Description}\""));

            foreach (var kv in configurable.Configuration)
            {
                root.Add(kv.Key, kv.Value.Accept(new ExConfigItemJsonConverterVisitor()));
            }

            return root;
        }
    }
}