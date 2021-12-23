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
using Exchange.Model.Statistics.Interfaces;

namespace Exchange.Model.Statistics
{
    /// <summary>
    ///     Modell für den Aktoransicht (logische zuweisung).
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissModel" />
    /// <seealso cref="Exchange.Model.Statistics.Interfaces.IExFinalView" />
    public class ExActorView : IBissModel, IExFinalView
    {
        #region Properties

        /// <summary>
        ///     Der Id vom final Unteransicht, dem dieses Instanz zugehört.
        /// </summary>
        public long? FinalSubViewId { get; set; }

        /// <summary>
        ///     Der Name von diesem Instanz.
        /// </summary>
        public string Name { get; set; }

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