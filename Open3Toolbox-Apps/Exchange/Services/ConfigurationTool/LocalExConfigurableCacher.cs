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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Exchange.Model.ConfigurationTool;
using Exchange.Services.ConfigurationTool.Interfaces;

namespace Exchange.Services.ConfigurationTool
{
    /// <summary>
    ///     Lokale Version vom <see cref="IExConfigurableCacher" />
    /// </summary>
    /// <seealso cref="Exchange.Services.ConfigurationTool.Interfaces.IExConfigurableCacher" />
    public class LocalExConfigurableCacher : IExConfigurableCacher
    {
        /// <summary>
        ///     Die Projekt Vorlagen.
        /// </summary>
        private static readonly ConcurrentBag<ExProject> _projectTemplates = new ConcurrentBag<ExProject>();

        /// <summary>
        ///     Die Gateway Vorlagen.
        /// </summary>
        private static readonly ConcurrentBag<ExGateway> _gatewayTemplates = new ConcurrentBag<ExGateway>();

        /// <summary>
        ///     Die Sensor Vorlagen.
        /// </summary>
        private static readonly ConcurrentBag<ExSensor> _sensorTemplates = new ConcurrentBag<ExSensor>();

        #region Interface Implementations

        /// <summary>
        ///     Cache-t den Projekt Implementation zu.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        public void AddProjectImplementation(ExProject project) =>
            _projectTemplates.Add(project);

        /// <summary>
        ///     Cache-t den Gateway Implementation zu.
        /// </summary>
        /// <param name="gateway">Der Gateway.</param>
        public void AddGatewayImplementation(ExGateway gateway) =>
            _gatewayTemplates.Add(gateway);

        /// <summary>
        ///     Cache-t den Sensor Implementation zu.
        /// </summary>
        /// <param name="sensor">Der Sensor.</param>
        public void AddSensorImplementation(ExSensor sensor) =>
            _sensorTemplates.Add(sensor);

        /// <summary>
        ///     Holt die konfigurierbaren Gateway Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Gateway Implementationen.
        /// </returns>
        public IReadOnlyCollection<ExGateway> GetGatewayImplementations() =>
            _gatewayTemplates;

        /// <summary>
        ///     Holt die konfigurierbaren Projekt Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Projekt Implementationen.
        /// </returns>
        public IReadOnlyCollection<ExProject> GetProjectImplementations() =>
            _projectTemplates;

        /// <summary>
        ///     Holt die konfigurierbaren Sensor Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Sensor Implementationen.
        /// </returns>
        public IReadOnlyCollection<ExSensor> GetSensorImplementations() =>
            _sensorTemplates;

        #endregion
    }
}