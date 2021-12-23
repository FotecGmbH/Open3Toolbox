// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Matthias Mandl
// Project:       GatewayNetworkHandler
// 
// Released under MIT

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ExchangeLibrary.ExchangeData;

namespace GatewayNetworkHandler
{
    /// <summary>
    ///     The Server for Sensors handles all incoming connections from Sensors
    ///     It starts to listen to them and get the Data sent by them
    /// </summary>
    public interface IServerForSensors
    {
        #region Properties

        /// <summary>
        ///     All collected values from the sensor
        /// </summary>
        public ConcurrentQueue<MeasurementValue> Values { get; }

        #endregion

        /// <summary>
        ///     Starts the Server listening to all incoming Connections and get the Data from them
        /// </summary>
        /// <returns>A Task</returns>
        public Task StartAsync();
    }
}