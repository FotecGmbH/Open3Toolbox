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

namespace Exchange.Model
{
    /// <summary>
    ///     <para>Akteulle ViewInfo</para>
    ///     Klasse ExViewState. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExViewState : IBissModel
    {
        #region Properties

        /// <summary>
        ///     ViewName
        /// </summary>
        public string ViewName { get; set; } = string.Empty;

        /// <summary>
        ///     Datum
        /// </summary>
        public DateTime CurrentDateTime { get; set; }

        /// <summary>
        ///     IsAppearing - sonst Disappearing
        /// </summary>
        public bool IsAppearing { get; set; }

        #endregion

        #region Interface Implementations

#pragma warning disable 0067
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        #endregion
    }
}