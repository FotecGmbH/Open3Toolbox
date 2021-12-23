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

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeLibrary;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace GatewayNetworkHandler
{
    using System;

    /// <summary>
    ///     Gatewayhandler holds the REST communication to Server and TCP comm to sensors
    ///     It interchanges the info which should be interchanged between these 2
    /// </summary>
    public class GatewayHandler
    {
        /// <summary>
        ///     The Client for the Server.
        /// </summary>
        private readonly IClientForServer _clientForServer;

        /// <summary>
        ///     The connection where to get the id
        /// </summary>
        private readonly IConnectionForId? _connectionForId;

        /// <summary>
        ///     The locker for the class.
        /// </summary>
        private readonly object _locker;

        /// <summary>
        ///     The listener/server for the Sensors.
        /// </summary>
        private readonly IServerForSensors _serverForSensors;

        /// <summary>
        ///     The gateway.
        /// </summary>
        private Gateway? _gateway;

        /// <summary>
        ///     Initializes a new instance of the gateway handler
        /// </summary>
        /// <param name="locker">The locker for the class.</param>
        /// <param name="clientForServer">The Client for the Server.</param>
        /// <param name="serverForSensors">The listener/server for the Sensors.</param>
        /// <param name="connectionForId">The connection for getting the ID.</param>
        public GatewayHandler(object locker, IClientForServer clientForServer, IServerForSensors serverForSensors, IConnectionForId connectionForId)
        {
            _locker = locker;
            _clientForServer = clientForServer ?? throw new ArgumentNullException(nameof(clientForServer));
            _serverForSensors = serverForSensors ?? throw new ArgumentNullException(nameof(serverForSensors));
            _connectionForId = connectionForId ?? throw new ArgumentNullException(nameof(connectionForId));
        }

        /// <summary>
        ///     Initializes a new instance of the gateway handler
        /// </summary>
        /// <param name="locker">The locker for the class.</param>
        /// <param name="clientForServer">The Client for the Server.</param>
        /// <param name="serverForSensors">The listener/server for the Sensors.</param>
        /// <param name="gateway">The Gateway.</param>
        public GatewayHandler(object locker, IClientForServer clientForServer, IServerForSensors serverForSensors, Gateway gateway)
        {
            _locker = locker;
            _clientForServer = clientForServer ?? throw new ArgumentNullException(nameof(clientForServer));
            _serverForSensors = serverForSensors ?? throw new ArgumentNullException(nameof(serverForSensors));
            Gateway = gateway ?? throw new ArgumentNullException(nameof(gateway));
        }

        #region Properties

        /// <summary>
        ///     The gateway.
        /// </summary>
        private Gateway Gateway
        {
            get => _gateway ?? throw new ArgumentNullException(nameof(Gateway));
            set => _gateway = value ?? throw new ArgumentNullException(nameof(Gateway));
        }

        /// <summary>
        ///     Indicates if this class should stop sending messages to the server or not.
        /// </summary>
        public bool StopSendingToServer { get; set; }

        #endregion

        /// <summary>
        ///     It gets data from the server,
        ///     starts a task sending data to the server in an intervall and
        ///     starts a task listening for new sensors which want to connect
        /// </summary>
        /// <returns>A Task</returns>
        public async Task StartAsync()
        {
            List<Task> tasks = new List<Task>();
            if (_connectionForId == null)
            {
                throw new ArgumentException("The connection for getting an id was not declared");
            }

            tasks.Add(_connectionForId.StartAsync(new Command(async obj =>
            {
                var id = (long) obj;
                List<Task> tasks = new List<Task>();
                Gateway = await _clientForServer.GetGatewayDataAsync(id.ToString()).ConfigureAwait(true);
                //Gateway.Sensors.ToList().ForEach(sensor => _serverForSensors.IdToSensorJsonStrings.Add(sensor.SensorId, sensor.ToJson()));
                tasks.Add(_serverForSensors.StartAsync());
            })));
            await Task.WhenAll(tasks).ConfigureAwait(true);


            while (true)
            {
                Thread.Sleep(500);
            }
        }

        /// <summary>
        ///     starts a task sending data to the server in an intervall and
        ///     starts a task listening for new sensors which want to connect
        /// </summary>
        /// <returns>A Task</returns>
        public async Task StartAsyncSimple()
        {
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Factory.StartNew(() => _serverForSensors.StartAsync()));

            await Task.WhenAll(tasks).ConfigureAwait(true);
            while (true)
            {
                Thread.Sleep(500);
            }
        }
    }
}