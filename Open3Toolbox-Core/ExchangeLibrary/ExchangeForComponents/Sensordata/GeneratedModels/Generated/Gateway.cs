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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Biss.Interfaces;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.DTOs;
using Newtonsoft.Json;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     A gateway, contained by a project.
    /// </summary>
    [GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    public class Gateway : IInputDTO<TablePublishedGateway>, IOutputDTO<TablePublishedGateway>, IBissSerialize
    {
        #region Properties

        /// <summary>
        ///     The name of the gateway.
        /// </summary>
        [JsonProperty("name", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public string Name { get; set; }

        /// <summary>
        ///     The communication, how the gateway communicates with the sensors
        /// </summary>
        [JsonProperty("comToSens", Required = Required.Always)]
        public Comunication ComToSens { get; set; }

        /// <summary>
        ///     The id of the sensor.
        /// </summary>
        [JsonProperty("id", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public long Id { get; set; }

        /// <summary>
        ///     The description of the gateway.
        /// </summary>
        [JsonProperty("description", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        ///     The address of the uplink-server.
        /// </summary>
        [JsonProperty("serverUrl", Required = Required.Always)]
        [Required(AllowEmptyStrings = true)]
        public Uri ServerUrl { get; set; }

        /// <summary>
        ///     The interval, how often the gateway sends data.
        /// </summary>
        [JsonProperty("interval", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [Range(0, int.MaxValue)]
        public int Interval { get; set; }

        /// <summary>
        ///     The sensors of the gateway.
        /// </summary>
        [JsonProperty("sensors", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<Sensor> Sensors { get; set; }

        #endregion
    }

    /// <summary>
    ///     The communication how to communicate
    /// </summary>
    public enum Comunication
    {
        Ethernet,
        Serial
    }
}