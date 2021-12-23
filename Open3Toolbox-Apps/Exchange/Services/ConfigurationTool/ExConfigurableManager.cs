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
using System.Collections.Generic;
using System.Linq;
using Exchange.Model.ConfigurationTool;
using Exchange.Model.ConfigurationTool.Interfaces;
using Exchange.Services.ConfigurationTool.Interfaces;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;
using ExConfigExchange.Services;
using ExConfigExchange.Services.Interfaces;
using IX.Observable;

namespace Exchange.Services.ConfigurationTool
{
    /// <summary>
    ///     Konkrete Manager für konfigurierbare Modelle.
    /// </summary>
    /// <seealso cref="Exchange.Services.ConfigurationTool.Interfaces.IExConfigurableManager" />
    public class ExConfigurableManager : IExConfigurableManager
    {
        /// <summary>
        ///     Der configuration item manager.
        /// </summary>
        private readonly IExConfigItemManager _configItemManager;

        /// <summary>
        ///     Der cacher von Templates
        /// </summary>
        private readonly IExConfigurableCacher cacher;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExConfigurableManager" /> class.
        /// </summary>
        /// <param name="configItemManager">The configuration item manager.</param>
        /// <param name="cacher">The cacher.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     configItemManager
        ///     or
        ///     cacher
        /// </exception>
        public ExConfigurableManager(IExConfigItemManager configItemManager, IExConfigurableCacher cacher)
        {
            _configItemManager = configItemManager ?? throw new ArgumentNullException(nameof(configItemManager));
            this.cacher = cacher ?? throw new ArgumentNullException(nameof(cacher));
        }

        /// <summary>
        ///     Liest die Properties aus dem angegebenen <typeparamref name="T" /> und setzt die werte falls einen basis wert
        ///     vorhanden sei.
        /// </summary>
        /// <typeparam name="T">Der <see cref="Type" /> von der basis wert.</typeparam>
        /// <param name="defaultValue">Der basis wert.</param>
        /// <returns>
        ///     Properties die aus der originallen modell durch reflection ausgelesen wurden.
        ///     {Schlüssel:"Property Name"; Wert:"Einen <see cref="IExConfigItem" /> instanz mit dem richtigen Type."}
        /// </returns>
        private ObservableDictionary<string, IExConfigItem> GetConfigurationFor<T>(T defaultValue = null) where T : class
        {
            var props = typeof(T).GetProperties().Where(tp => !typeof(IExConfigurable).GetProperties().Any(cp => tp.PropertyType == cp.PropertyType && tp.Name == cp.Name));
            var configs = new ObservableDictionary<string, IExConfigItem>();
            foreach (var prop in props)
            {
                // Ausnahmen
                if (
                    prop.Name == nameof(Project.Id)
                    || prop.Name == nameof(Gateway.Id)
                    || prop.Name == nameof(Sensor.Id)
                    //|| prop.Name == nameof(Sensor.SensorId)
                    || prop.Name == nameof(Gateway.Sensors)
                    || prop.Name == nameof(Project.Gateways)
                )
                {
                    continue;
                }

                var displayKey = prop.GetDisplayKey() ?? prop.Name;
                var propType = prop.ShouldConfigureAs() ?? prop.PropertyType;

                object? value = null;
                if (defaultValue != null)
                {
                    value = prop.GetValue(defaultValue);
                }

                var config = _configItemManager.GetIExConfigItemFrom(displayKey, propType, value, !prop.CanWrite || prop.IsReadOnly());
                config.Hidden = prop.IsHidden();
                if (config is ExObjectConfigItem temp)
                {
                    temp.HadConfigureAsAttribute = prop.ShouldConfigureAs() != null;
                }

                var key = prop.GetJsonPropertyName() ?? prop.Name;
                configs.Add(key, config);
            }

            return configs;
        }

        #region Interface Implementations

        /// <summary>
        ///     Konvertiert den Project in seinem konfigurierbaren Version.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        /// <returns>
        ///     Konfigurierbaren Version vom Projekt.
        /// </returns>
        public ExProject ConvertProject(Project project)
        {
            var toReturn = new ExProject();
            toReturn.Name = project.Name;
            toReturn.Description = project.Description;
            toReturn.Type = "Project";
            toReturn.Configuration = GetConfigurationFor(project);

            return toReturn;
        }

        /// <summary>
        ///     Konvertiert den Gateway in seinem konfigurierbaren Version.
        /// </summary>
        /// <param name="projectId">Der Projekt Id, dem der Gateway zugehört.</param>
        /// <param name="gateway">Der Gateway.</param>
        /// <returns>
        ///     Konfigurierbaren Version vom Gateway.
        /// </returns>
        public ExGateway ConvertGateway(long projectId, Gateway gateway)
        {
            var toReturn = new ExGateway();
            toReturn.ProjectId = projectId;
            toReturn.Name = gateway.Name;
            toReturn.Description = gateway.Description;
            toReturn.Type = "Gateway";
            toReturn.Configuration = GetConfigurationFor(gateway);

            return toReturn;
        }

        /// <summary>
        ///     Konvertiert den Sensor in seinem konfigurierbaren Version.
        /// </summary>
        /// <param name="gatewayId">Der Gateway Id, dem der Sensor zugehört.</param>
        /// <param name="sensor">Der Sensor.</param>
        /// <returns>
        ///     Konfigurierbaren Version vom Sensor.
        /// </returns>
        public ExSensor ConvertSensor(long gatewayId, Sensor sensor)
        {
            var toReturn = new ExSensor();
            toReturn.GatewayId = gatewayId;
            toReturn.Name = sensor.Name;
            toReturn.Description = sensor.Description;
            toReturn.Type = "Sensor";
            toReturn.Configuration = GetConfigurationFor(sensor);

            return toReturn;
        }

        /// <summary>
        ///     Holt die konfigurierbaren Projekt Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Projekt Implementationen.
        /// </returns>
        public IEnumerable<ExProject> GetProjectImplementations()
        {
            // For now..
            if (cacher.GetProjectImplementations().Count == 0)
            {
                cacher.AddProjectImplementation(new ExProject
                                                {
                                                    Name = "New Project",
                                                    Type = "Project",
                                                    Configuration = GetConfigurationFor<Project>()
                                                });
            }

            return cacher.GetProjectImplementations();
        }

        /// <summary>
        ///     Holt die konfigurierbaren Gateway Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Gateway Implementationen.
        /// </returns>
        public IEnumerable<ExGateway> GetGatewayImplementations()
        {
            if (cacher.GetGatewayImplementations().Count == 0)
            {
                cacher.AddGatewayImplementation(new ExGateway
                                                {
                                                    Name = "New Gateway",
                                                    Type = "Gateway",
                                                    Configuration = GetConfigurationFor<Gateway>()
                                                });
            }

            return cacher.GetGatewayImplementations();
        }

        /// <summary>
        ///     Holt die konfigurierbaren Sensor Implementationen.
        /// </summary>
        /// <returns>
        ///     Die konfigurierbaren Sensor Implementationen.
        /// </returns>
        public IEnumerable<ExSensor> GetSensorImplementations()
        {
            if (cacher.GetSensorImplementations().Count == 0)
            {
                cacher.AddSensorImplementation(
                    new ExSensor
                    {
                        Name = "New Sensor",
                        Type = "Sensor",
                        Configuration = GetConfigurationFor<Sensor>()
                    });
            }

            return cacher.GetSensorImplementations();
        }

        #endregion
    }
}