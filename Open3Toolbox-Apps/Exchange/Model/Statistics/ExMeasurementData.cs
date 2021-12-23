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
    ///     Modell für einen Messungsdatei.
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissModel" />
    public class ExMeasurementData : IBissModel
    {
        #region Properties

        /// <summary>
        ///     Id der Messung.
        /// </summary>
        public long MeasurementId { get; set; }

        /// <summary>
        ///     Wert der Messung.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        ///     Zeitpunkt der Messung.
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///     Längengrad des Sensors, dem der Messung zugehört.
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        ///     Breitengrad des Sensors, dem der Messung zugehört.
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        ///     Höhenlage des Sensors, dem der Messung zugehört.
        /// </summary>
        public float Altitude { get; set; }

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