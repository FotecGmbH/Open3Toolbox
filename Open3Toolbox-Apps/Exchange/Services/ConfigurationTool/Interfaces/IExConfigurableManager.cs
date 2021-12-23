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
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace Exchange.Services.ConfigurationTool.Interfaces
{
    /// <summary>
    ///     Manager für konfigurierbare Modelle.
    /// </summary>
    public interface IExConfigurableManager
    {
        /// <summary>
        ///     Konvertiert den Project in seinem konfigurierbaren Version.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        /// <returns>
        ///     Konfigurierbaren Version vom Projekt.
        /// </returns>
        ExProject ConvertProject(Project project);

        /// <summary>
        ///     Konvertiert den Gateway in seinem konfigurierbaren Version.
        /// </summary>
        /// <param name="projectId">Der Projekt Id, dem der Gateway zugehört.</param>
        /// <param name="gateway">Der Gateway.</param>
        /// <returns>
        ///     Konfigurierbaren Version vom Gateway.
        /// </returns>
        public ExGateway ConvertGateway(long projectId, Gateway gateway);

        /// <summary>
        ///     Konvertiert den Sensor in seinem konfigurierbaren Version.
        /// </summary>
        /// <param name="gatewayId">Der Gateway Id, dem der Sensor zugehört.</param>
        /// <param name="sensor">Der Sensor.</param>
        /// <returns>
        ///     Konfigurierbaren Version vom Sensor.
        /// </returns>
        public ExSensor ConvertSensor(long gatewayId, Sensor sensor);

        /// <summary>
        ///     Holt die konfigurierbaren Projekt Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Projekt Implementationen.
        /// </returns>
        IEnumerable<ExProject> GetProjectImplementations();

        /// <summary>
        ///     Holt die konfigurierbaren Gateway Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Gateway Implementationen.
        /// </returns>
        IEnumerable<ExGateway> GetGatewayImplementations();

        /// <summary>
        ///     Holt die konfigurierbaren Sensor Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Sensor Implementationen.
        /// </returns>
        IEnumerable<ExSensor> GetSensorImplementations();
    }
}