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
using ExConfigExchange.Models.Interfaces;
using IX.Observable;

namespace Exchange.Model.ConfigurationTool.Interfaces
{
    /// <summary>
    ///     Diese Interface ist für die Konfigurable aber Konkrete modelle gedacht.
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissSerialize" />
    public interface IExConfigurable : IBissSerialize, INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        ///     Die <see cref="Type.FullName" /> des Models.
        /// </summary>
        string Type { get; set; }

        /// <summary>
        ///     Der angezeigte Name, es ist nur für den Benutzer wichtig.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Die Beschreibung, es ist nur für den Benutzer wichtig.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        ///     Properties die aus der originallen modell durch reflection ausgelesen wurden. (Sehe
        ///     <see cref="Services.ConfigurationTool.ExConfigurableManager" />)
        ///     {Schlüssel:"Property Name"; Wert:"Einen <see cref="IExConfigItem" /> instanz mit dem richtigen Type."}
        /// </summary>
        ObservableDictionary<string, IExConfigItem> Configuration { get; set; }

        #endregion
    }
}