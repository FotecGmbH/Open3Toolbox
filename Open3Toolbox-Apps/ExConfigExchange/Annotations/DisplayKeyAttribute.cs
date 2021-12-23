// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    /// Setzt den DisplayKey des Field/Property/Enum/Class/Interface/Struct.
    /// Sollte der DisplayKey nicht übersetzbar sein, wird der <see cref="DisplayKeyAttribute.DisplayKey" /> sowie es ist angezeigt.
    /// Priority Type > Property/Field.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Enum | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class DisplayKeyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayKeyAttribute"/> class.
        /// </summary>
        /// <param name="displayKey">The display key.</param>
        public DisplayKeyAttribute(string displayKey)
        {
            DisplayKey = displayKey;
        }

        /// <summary>
        /// Der Display Key.
        /// </summary>
        public string DisplayKey { get; }
    }
}
