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

using System.ComponentModel;
using Biss.Interfaces;
using Exchange.Enum;

namespace Exchange.Model.Statistics
{
    /// <summary>
    ///     Modell für die Einzelheiten von Aktor.
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissModel" />
    public class ExActorDetails : IBissModel
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
        ///     Die Beschreibung von diesem Instanz.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     Wie <see cref="Value" /> gesetzt werden soll. (Switch oder Range.)
        /// </summary>
        public ActorSetterType SetterType { get; set; }

        /// <summary>
        ///     Der Wert von diesem Instanz, zwischen 0 und 1.
        /// </summary>
        public double Value { get; set; }

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