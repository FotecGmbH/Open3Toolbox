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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Biss.Serialize;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace GatewayNetworkHandler
{
    /// <summary>
    ///     This class initializes a gateway handler and
    ///     determine, which connections it has
    /// </summary>
    public static class GatewayHandlerInitializer
    {
        /// <summary>
        ///     Starts a gatewayhandler with a REST-API Server connection
        ///     and a Udp-Sensor Connection
        /// </summary>
        /// <param name="configs">The configuration of the gateway</param>
        /// <returns>A Task</returns>
        public static async Task StartAsync4(Gateway configs)
        {
            if (configs == null)
            {
                throw new ArgumentNullException(nameof(configs));
            }

            object locker = new object();

            UdpServerForSensors udpServerForSensors = new UdpServerForSensors(configs.Sensors.ToList(), locker);
            using RestClientForServer restClientForServer = new RestClientForServer(configs.ServerUrl, locker, udpServerForSensors.Values);
            GatewayHandler gatewayHandler = new GatewayHandler(locker, restClientForServer, udpServerForSensors, configs);
            await gatewayHandler.StartAsyncSimple().ConfigureAwait(true);
        }

        /// <summary>
        ///     Starts a gatewayhandler with a REST-API Server connection
        ///     and a Serial-Sensor Connection
        /// </summary>
        /// <param name="configs">The configuration of the gateway</param>
        /// <returns>A Task</returns>
        public static async Task StartAsync5(Gateway configs)
        {
            if (configs == null)
            {
                throw new ArgumentNullException(nameof(configs));
            }

            object locker = new object();

            SerialServerForSensors serialServerForSensors = new SerialServerForSensors(configs.Sensors.ToList(), locker);
            using RestClientForServer restClientForServer = new RestClientForServer(configs.ServerUrl, locker, serialServerForSensors.Values);
            GatewayHandler gatewayHandler = new GatewayHandler(locker, restClientForServer, serialServerForSensors, configs);
            await gatewayHandler.StartAsyncSimple().ConfigureAwait(true);
        }

        /// <summary>
        ///     Gets the configuration of the gateway from the file
        /// </summary>
        /// <returns>The configuration of the gateway</returns>
        public static async Task<Gateway> GetConfigs()
        {
            //Directory.SetCurrentDirectory(Directory.GetCurrentDirectory() + @"\..\..\..\..\..\Dataskop-Apps\");
            string path = Directory.GetCurrentDirectory() + @"\..\..\..\..\..\Open3Toolbox-Apps\gateConfigs.json";
            string text = await File.ReadAllTextAsync(path, CancellationToken.None).ConfigureAwait(true);

            return BissDeserialize.FromJson<Gateway>(text);
        }
    }
}