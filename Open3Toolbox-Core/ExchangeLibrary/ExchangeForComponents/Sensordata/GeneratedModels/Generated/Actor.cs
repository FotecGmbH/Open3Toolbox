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
    ///     Represents an actuator
    /// </summary>
    public class Actor
    {
        #region Properties

        /// <summary>
        ///     Name of the actor
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        [DisplayNameProperty]
        public string Name { get; set; }

        /// <summary>
        ///     Id of the actor
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        public long Id { get; set; }

        /// <summary>
        ///     Port on which the actor is attached
        /// </summary>
        [JsonProperty("port", Required = Required.Always)]
        public int Port { get; set; }

        /// <summary>
        ///     The opcodes how to write to this actor
        /// </summary>
        [Hidden]
        [JsonProperty("writeOpCodes")]
        public List<byte> WriteOpCodes { get; set; } = new List<byte>();

        #endregion
    }
}