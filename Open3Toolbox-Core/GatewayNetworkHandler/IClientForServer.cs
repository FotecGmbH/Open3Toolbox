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
using System.Threading.Tasks;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace GatewayNetworkHandler
{
    /// <summary>
    ///     The client for server is responsible
    ///     for the Communication to the server.
    /// </summary>
    public interface IClientForServer : IDisposable
    {
        /// <summary>
        ///     Gets the gateway data from the server.
        ///     You need the id to get detected by the server.
        /// </summary>
        /// <param name="id">The id to get detected by the server</param>
        /// <returns>The deserialized gateway from the server.</returns>
        public Task<Gateway> GetGatewayDataAsync(string id);
    }
}