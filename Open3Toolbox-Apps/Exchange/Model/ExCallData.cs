// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System;
using System.ComponentModel;
using Biss.Interfaces;
using Exchange.Enum;

namespace Exchange.Model
{
    /// <summary>
    ///     <para>Model für Anrufdaten</para>
    ///     Klasse ExCallData. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExCallData : IBissModel
    {
        #region Properties

        /// <summary>
        ///     Die Zeit des Anrufs.
        /// </summary>
        public DateTime CallDateTime { get; set; }

        /// <summary>
        ///     Der Name des Anrufers.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Die Telefonnummer des Anrufers.
        /// </summary>
        public string CallNumber { get; set; } = string.Empty;

        /// <summary>
        ///     Anrufart
        /// </summary>
        public EnumCallType CallType { get; set; }

        #endregion

        #region Interface Implementations

#pragma warning disable 0067
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        #endregion
    }
}