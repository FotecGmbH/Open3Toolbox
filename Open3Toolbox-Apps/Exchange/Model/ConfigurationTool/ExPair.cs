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

using System.Collections.Generic;
using System.ComponentModel;
using Biss.Interfaces;

namespace Exchange.Model.ConfigurationTool
{
    /// <summary>
    ///     Datenträger für Paaren (Echte Gateways/Sensoren <-> Logischen Gateways/Sensoren).
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissSerialize" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class ExPair : IBissSerialize, INotifyPropertyChanged
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExPair" /> class.
        /// </summary>
        public ExPair()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExPair" /> class.
        /// </summary>
        /// <param name="kv">Das Paar.</param>
        public ExPair(KeyValuePair<string, long> kv)
        {
            Key = kv.Key;
            Value = kv.Value;
        }

        #region Properties

        /// <summary>
        ///     UID vom echten Gateway/Sensor.
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        ///     ID vom logischen Gateway/Sensor.
        /// </summary>
        public long Value { get; set; }

        #endregion

        #region Interface Implementations

#pragma warning disable CS0414
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged = null!;
#pragma warning restore CS0414

        #endregion
    }
}