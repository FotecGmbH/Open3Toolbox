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
using ExchangeLibrary.SensorData.Visitors;
using ExConfigExchange.Annotations;
using Newtonsoft.Json;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     Represents a gpio interface
    /// </summary>
    public class GpioInterface : CommunicationInterface
    {
        #region Properties

        /// <summary>
        ///     The interface type
        /// </summary>
        [JsonProperty("interfaceType",
            NullValueHandling = NullValueHandling.Ignore)]
        [ReadOnly]
        [Hidden]
        public string InterfaceType { get; set; } = "gpioInterface";

        /// <summary>
        ///     The gpio chips of the interface
        /// </summary>
        [JsonProperty("chips", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<GpioChip> GpioChips { get; set; }

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