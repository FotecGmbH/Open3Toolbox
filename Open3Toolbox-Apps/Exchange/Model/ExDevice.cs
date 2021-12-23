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
using Biss.Apps.Model;

namespace Exchange.Model
{
    /// <summary>
    ///     <para>ExDevice</para>
    ///     Klasse ExDevice. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExDevice : DeviceBase
    {
        #region Properties

        /// <summary>
        ///     Gerät gesperrt
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        ///     Abweichung zu UTC Zeit
        /// </summary>
        public TimeSpan TimeDifference { get; set; }

        #endregion
    }
}