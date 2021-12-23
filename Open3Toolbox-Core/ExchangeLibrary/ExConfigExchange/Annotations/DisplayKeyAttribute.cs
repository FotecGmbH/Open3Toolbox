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

namespace ExConfigExchange.Annotations
{
    /// <summary>
    ///     Setzt den DisplayKey des Field/Property/Enum/Class/Interface/Struct.
    ///     Sollte der DisplayKey nicht übersetzbar sein, wird der <see cref="DisplayKeyAttribute.DisplayKey" /> sowie es ist
    ///     angezeigt.
    ///     Priority Type > Property/Field.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class DisplayKeyAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DisplayKeyAttribute" /> class.
        /// </summary>
        /// <param name="displayKey">The display key.</param>
        public DisplayKeyAttribute(string displayKey)
        {
            DisplayKey = displayKey;
        }

        #region Properties

        /// <summary>
        ///     Der Display Key.
        /// </summary>
        public string DisplayKey { get; }

        #endregion
    }
}