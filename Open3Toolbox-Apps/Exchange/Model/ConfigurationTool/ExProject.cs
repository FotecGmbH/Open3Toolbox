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
using Exchange.Model.ConfigurationTool.Interfaces;
using ExConfigExchange.Models.Interfaces;
using IX.Observable;

namespace Exchange.Model.ConfigurationTool
{
    /// <summary>
    ///     Der konfigurierbaren Projekt Modell.
    /// </summary>
    /// <seealso cref="Exchange.Model.ConfigurationTool.Interfaces.IExConfigurable" />
    public class ExProject : IExConfigurable
    {
        #region Properties

        /// <summary>
        ///     Die <see cref="System.Type.FullName" /> des Models.
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        ///     Der angezeigte Name, es ist nur für den Benutzer wichtig.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Die Beschreibung, es ist nur für den Benutzer wichtig.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        ///     Properties die aus der originallen modell durch reflection ausgelesen wurden. (Sehe
        ///     <see cref="Services.ConfigurationTool.ExConfigurableJsonConverter" />)
        ///     {Schlüssel:"Property Name"; Wert:"Einen <see cref="IExConfigItem" /> instanz mit dem richtigen Type."}
        /// </summary>
        public ObservableDictionary<string, IExConfigItem> Configuration { get; set; } = new ObservableDictionary<string, IExConfigItem>();

        #endregion

        #region Interface Implementations

#pragma warning disable CS0414
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged = null!;
#pragma warning restore CS0414

        #endregion
    }
}