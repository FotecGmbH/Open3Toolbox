// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Matthias Mandl
// Project:       PCGateway
// 
// Released under MIT

using System.Threading.Tasks;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using GatewayNetworkHandler;

namespace PCGateway
{
    using System;

    /// <summary>
    ///     Entry point of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Entrypoint of the Applikation
        /// </summary>
        /// <param name="args">CLI arguments</param>
        /// <returns>A Task</returns>
        static async Task Main(string[] args)
        {
            var configs = await GatewayHandlerInitializer.GetConfigs().ConfigureAwait(true);

            switch (configs.ComToSens)
            {
                case Comunication.Serial:
                    await GatewayHandlerInitializer.StartAsync5(configs).ConfigureAwait(true);
                    break;
                case Comunication.Ethernet:
                    await GatewayHandlerInitializer.StartAsync4(configs).ConfigureAwait(true);
                    break;
                default:
                    throw new ArgumentException("this comunication is not supported");
            }
        }
    }
}