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
using Exchange.Model.ConfigurationTool;

namespace Exchange.Services.ConfigurationTool.Interfaces
{
    /// <summary>
    ///     Cacher für konfigurierbaren Vorlagen.
    /// </summary>
    public interface IExConfigurableCacher
    {
        /// <summary>
        ///     Cache-t den Projekt Implementation zu.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        void AddProjectImplementation(ExProject project);

        /// <summary>
        ///     Cache-t den Gateway Implementation zu.
        /// </summary>
        /// <param name="gateway">Der Gateway.</param>
        void AddGatewayImplementation(ExGateway gateway);

        /// <summary>
        ///     Cache-t den Sensor Implementation zu.
        /// </summary>
        /// <param name="sensor">Der Sensor.</param>
        void AddSensorImplementation(ExSensor sensor);

        /// <summary>
        ///     Holt die konfigurierbaren Projekt Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Projekt Implementationen.
        /// </returns>
        IReadOnlyCollection<ExProject> GetProjectImplementations();

        /// <summary>
        ///     Holt die konfigurierbaren Gateway Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Gateway Implementationen.
        /// </returns>
        IReadOnlyCollection<ExGateway> GetGatewayImplementations();

        /// <summary>
        ///     Holt die konfigurierbaren Sensor Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Sensor Implementationen.
        /// </returns>
        IReadOnlyCollection<ExSensor> GetSensorImplementations();
    }
}