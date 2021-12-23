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

namespace Exchange.Model.Statistics
{
    /// <summary>
    ///     Modell für die Einzelheiten von Messung.
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissModel" />
    public class ExMeasurementDetails : IBissModel
    {
        #region Properties

        /// <summary>
        ///     Der Name von diesem Instanz.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Die Beschreibung von diesem Instanz.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        ///     Die Port von diesem Instanz.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     Zuletzt gemesste Wert.
        /// </summary>
        public double LastMeasuredValue { get; set; }

        /// <summary>
        ///     Zeitpunkt der letzten Messung.
        /// </summary>
        public DateTime? LastMeasured { get; set; }

        #endregion

        #region Interface Implementations

#pragma warning disable CS0414
        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = null!;
#pragma warning restore CS0414

        #endregion
    }
}