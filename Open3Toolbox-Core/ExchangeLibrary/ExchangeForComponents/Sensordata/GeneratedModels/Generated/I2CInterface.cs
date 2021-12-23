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
using ExchangeLibrary.SensorData.Visitors;
using ExConfigExchange.Annotations;
using Newtonsoft.Json;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     Represents a i2c interface
    /// </summary>
    public class I2CInterface : CommunicationInterface
    {
        #region Properties

        /// <summary>
        ///     The interface type
        /// </summary>
        [JsonProperty("interfaceType", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        [ReadOnly]
        [Hidden]
        public string InterfaceType { get; set; } = "i2cInterface";

        /// <summary>
        ///     The bus on which the i2c communication take place
        /// </summary>
        [JsonProperty("bus", Required = Required.Always)]
        [Required]
        [Hidden]
        public int BusId { get; set; }

        /// <summary>
        ///     The i2c chips of the interface
        /// </summary>
        [JsonProperty("chips", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<I2cChip> I2cChips { get; set; }

        #endregion

        /// <summary>
        ///     Accepts a visitor that this know which type he holds
        /// </summary>
        /// <param name="visitor">The visitor</param>
        public override void Accept(IInterfaceVisitor visitor)
        {
            if (visitor == null)
            {
                throw new ArgumentNullException(nameof(visitor));
            }

            visitor.Visit(this);
        }
    }
}