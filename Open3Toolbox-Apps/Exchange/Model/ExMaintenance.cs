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
using System.ComponentModel.DataAnnotations.Schema;
using Biss.Interfaces;
using Newtonsoft.Json;

namespace Exchange.Model
{
    /// <summary>
    ///     <para>Wartungsparameter</para>
    ///     Klasse ExMaintenance. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExMaintenance : IBissModel
    {
        private string _maintenanceText = string.Empty;

        #region Properties

        /// <summary>
        ///     Maximale Länge des Wartungstextes
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        public int MaxMaintenanceTextLength => 500;

        /// <summary>
        ///     Text während Wartungsarbeiten.
        /// </summary>
        public string MaintenanceText
        {
            get => _maintenanceText;
            set
            {
                if (value != null && value.Length > MaxMaintenanceTextLength)
                {
                    value = value.Substring(0, MaxMaintenanceTextLength);
                }

                _maintenanceText = value ?? string.Empty;
            }
        }

        /// <summary>
        ///     Ob Wartungstext in App angezeigt werden soll.
        /// </summary>
        public bool MaintenanceActive { get; set; }

        #endregion

        #region Interface Implementations

#pragma warning disable 0067
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        #endregion
    }
}