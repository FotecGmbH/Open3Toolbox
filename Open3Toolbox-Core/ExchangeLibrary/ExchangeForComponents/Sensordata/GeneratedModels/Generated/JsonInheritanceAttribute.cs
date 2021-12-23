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

namespace ExchangeLibrary.SensorData.GeneratedModels.Generated
{
    /// <summary>
    ///     The json inheritance attribute
    /// </summary>
    [GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    internal class JsonInheritanceAttribute : Attribute
    {
        /// <summary>
        ///     initializes a new instance
        /// </summary>
        /// <param name="key">The key of the attribute</param>
        /// <param name="type">The type of the attrobute</param>
        public JsonInheritanceAttribute(string key, Type type)
        {
            Key = key;
            Type = type;
        }

        #region Properties

        /// <summary>
        ///     The key of the attribute
        /// </summary>
        public string Key { get; }

        /// <summary>
        ///     The type of the attribute
        /// </summary>
        public Type Type { get; }

        #endregion
    }
}