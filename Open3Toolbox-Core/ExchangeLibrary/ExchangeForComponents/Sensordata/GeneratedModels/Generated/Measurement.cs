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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExConfigExchange.Annotations;
using Newtonsoft.Json;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     A measurement of a chip
    /// </summary>
    public class Measurement
    {
        #region Properties

        /// <summary>
        ///     Name of the measurement
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        [DisplayNameProperty]
        public string Name { get; set; }

        /// <summary>
        ///     Id of the measurement
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public long Id { get; set; }

        /// <summary>
        ///     The port, on which the measurement is attached
        /// </summary>
        [JsonProperty("port", Required = Required.Always)]
        public int Port { get; set; }

        /// <summary>
        ///     The read opcodes, how to read from the measurement
        /// </summary>
        [Hidden]
        [JsonProperty("readOpCodes")]
        public List<byte> ReadOpCodes { get; set; } = new List<byte>();

        #endregion
    }
}