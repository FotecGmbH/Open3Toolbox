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
using Biss.Interfaces;
using ExchangeLibrary.SensorData.Visitors;
using ExConfigExchange.Annotations;
using Newtonsoft.Json;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     Represents a communication interface, how to communiacte with a chip,
    ///     of a sensor
    /// </summary>
    [JsonConverter(typeof(JsonInheritanceConverter), "interfaceType")]
    [JsonInheritanceAttribute("gpioInterface", typeof(GpioInterface))]
    [JsonInheritanceAttribute("i2cInterface", typeof(I2CInterface))]
    [GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    [Interface]
    public class CommunicationInterface : IBissSerialize, IInterfaceVisitable
    {
        /// <summary>
        ///     Throws exception cause only an Implementation of the Interface should know how to
        ///     An Interface needs an Initialisation before it can work
        /// </summary>
        public virtual void Init() => throw new NotImplementedException();

        #region Interface Implementations

        /// <summary>
        ///     Throws exception cause only an Implementation of the Interface should give its type
        ///     Accepts a visitor so this know the type of the Interface
        /// </summary>
        /// <param name="visitor">The visitor</param>
        public virtual void Accept(IInterfaceVisitor visitor) => throw new NotImplementedException();

        #endregion
    }
}