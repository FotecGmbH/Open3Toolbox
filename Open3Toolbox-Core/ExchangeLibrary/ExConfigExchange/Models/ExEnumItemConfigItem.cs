// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:50
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using Biss.Interfaces;

namespace ExConfigExchange.Models
{
    /// <summary>
    ///     Representiert einen <see cref="Enum" />-Item für <see cref="ExEnumConfigItem" />.
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissSerialize" />
    public class ExEnumItemConfigItem : IBissSerialize
    {
        #region Properties

        /// <summary>
        ///     Mit Hilfe des Display-Keys sollte die Sprachspezifische (Englisch, Deutsch uws.) Name eines Objekts/Properties
        ///     geholt werden.
        ///     Dieses kann in abgebildeten Klassen mit Hilfe des <see cref="Annotations.DisplayKeyAttribute" /> festgelegt werden.
        /// </summary>
        /// <value>
        ///     Der display key.
        /// </value>
        public string DisplayKey { get; set; }

        /// <summary>
        ///     Integer-wert des <see cref="Enum" />-Items. (e.g.: <see cref="ConsoleColor.Cyan" /> == 11)
        /// </summary>
        public int Value { get; set; }

        #endregion
    }
}