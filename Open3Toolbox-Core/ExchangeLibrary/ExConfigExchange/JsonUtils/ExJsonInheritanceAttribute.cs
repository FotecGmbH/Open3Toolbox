// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:50
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.CodeDom.Compiler;

namespace ExConfigExchange.JsonUtils
{
    /// <summary>
    ///     Dieses Attribute mapt Kind klassen einer (abstract) Klasse/Interface zu deren konkrete implementationen bei der
    ///     Serializierung.
    ///     Bei serializierung wird der <see cref="ExJsonInheritanceConverter{TBase}" /> verwendet.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [GeneratedCode("NJsonSchema", "10.3.7.0 (Newtonsoft.Json v12.0.0.0)")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    internal class ExJsonInheritanceAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExJsonInheritanceAttribute" /> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="type">The type.</param>
        public ExJsonInheritanceAttribute(string key, Type type)
        {
            Key = key;
            Type = type;
        }

        #region Properties

        /// <summary>
        ///     Gets the key.
        /// </summary>
        /// <value>
        ///     The key.
        /// </value>
        public string Key { get; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        /// <value>
        ///     The type.
        /// </value>
        public Type Type { get; }

        #endregion
    }
}