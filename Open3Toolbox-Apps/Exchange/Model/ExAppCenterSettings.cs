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
    ///     <para>AppCenter Einstellungen</para>
    ///     Klasse ExAppCenterSettings. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExAppCenterSettings : IBissModel
    {
        #region Properties

        /// <summary>
        ///     Link für iOS App
        /// </summary>
        public string AppcenterLinkIos { get; set; } = string.Empty;

        /// <summary>
        ///     Link für Android App
        /// </summary>
        public string AppcenterLinkDroid { get; set; } = string.Empty;

        #endregion

        #region Interface Implementations

#pragma warning disable 0067
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        #endregion
    }
}