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

using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Biss.Interfaces;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.DTOs;
using ExConfigExchange.Annotations;
using Newtonsoft.Json;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     A sensor, contained by a gateway.
    /// </summary>
    [GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    public class Sensor : IBissSerialize, IInputDTO<TablePublishedSensor>, IOutputDTO<TablePublishedSensor>
    {
        #region Properties

        /// <summary>
        ///     The name of the sensor.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        /// <summary>
        ///     The id of the sensor.
        /// </summary>
        [JsonProperty("sensorId", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public long SensorId { get; set; }

        /// <summary>
        ///     The id of the sensor
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        [Hidden]
        public long Id { get; set; }

        /// <summary>
        ///     The description of the sensor.
        /// </summary>
        [JsonProperty("description", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        ///     The interval in milliseconds how often the measurements should measure
        /// </summary>
        [JsonProperty("measureInterval", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, int.MaxValue)]
        public int MeasureInterval { get; set; }

        /// <summary>
        ///     After how many times, the sensor should send its measurement values
        /// </summary>
        [JsonProperty("measureXTimesTillSend", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, int.MaxValue)]
        public byte MeasureXTimesTillSend { get; set; }

        /// <summary>
        ///     The communication interfaces, how to communicate
        ///     with the chips of the sensor.
        /// </summary>
        [JsonProperty("interfaces", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public List<CommunicationInterface> Interfaces { get; set; }

        /// <summary>
        ///     All opcodes of the interfaces/chips/measurements/actors how to run
        /// </summary>
        [Hidden]
        [JsonProperty("allOpCodes")]
        public List<byte> AllOpCodes { get; set; } = new List<byte>();

        #endregion
    }
}