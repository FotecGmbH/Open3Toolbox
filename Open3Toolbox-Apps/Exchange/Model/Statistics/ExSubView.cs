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
    ///     Modell für eine logische Unteransicht.
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissModel" />
    /// <seealso cref="Exchange.Model.Statistics.Interfaces.IExSubView" />
    public class ExSubView : IBissModel, IExSubView
    {
        #region Properties

        /// <summary>
        ///     Der Id vom Unteransicht, dem dieses Instanz zugehört. (Entweder es ist gesetzt oder <see cref="IsPartOfMainView" />
        ///     ist <c>true</c>)
        /// </summary>
        public long? SubViewId { get; set; }

        /// <summary>
        ///     <c>true</c> wenn teil des MainViews. (Entweder es ist <c>true</c> oder <see cref="SubViewId" /> ist gesetzt)
        /// </summary>
        public bool IsPartOfMainView { get; set; }

        /// <summary>
        ///     Der Name von diesem Instanz.
        /// </summary>
        public string Name { get; set; } = string.Empty;

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