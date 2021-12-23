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
using System.Runtime.Serialization;

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     The type of the sensor
    /// </summary>
    [GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    public enum SensorType
    {
        [EnumMember(Value = @"raspberry")] raspberry = 0,

        [EnumMember(Value = @"arduino")] arduino = 1,
    }
}