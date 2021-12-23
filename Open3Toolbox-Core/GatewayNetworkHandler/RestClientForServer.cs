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
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Biss.Serialize;
using ExchangeLibrary.ExchangeData;
using ExchangeLibrary.RestCommunication;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace GatewayNetworkHandler
{
    /// <summary>
    ///     The REST client for server is responsible
    ///     for the REST-communication to the server.
    /// </summary>
    public class RestClientForServer : IClientForServer
    {
        private readonly CancellationTokenSource _cts;

        /// <summary>
        ///     the sending tasks
        /// </summary>
        private readonly Task[] _sendTasks = new Task[3];

        /// <summary>
        ///     The server http client.
        /// </summary>
        private readonly ServerHttpClient _serverHttpClient;

        /// <summary>
        ///     The values, which should be sent to the server
        /// </summary>
        private readonly ConcurrentQueue<MeasurementValue> _values;


        /// <summary>
        ///     Initializes a new instance
        ///     of the REST-Client for server
        /// </summary>
        /// <param name="uri">The Uri, where to communicate.</param>
        /// <param name="locker">The locker</param>
        /// <param name="values">The values, which should be sent to the server </param>
        public RestClientForServer(Uri uri, object locker, ConcurrentQueue<MeasurementValue> values)
        {
            _serverHttpClient = new ServerHttpClient(uri);
            _cts = new CancellationTokenSource();
            _values = values;
            Init();
        }

        /// <summary>
        ///     Initializes the sending tasks
        /// </summary>
        public void Init()
        {
            for (var i = 0; i < _sendTasks.Length; i++)
            {
                _sendTasks[i] = SendingWorker(_cts.Token);
            }
        }

        /// <summary>
        ///     Sends all the specified values to the REST-server
        /// </summary>
        /// <param name="token">The cancelation token</param>
        private async Task SendingWorker(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!(_values is null) && _values.TryDequeue(out var value))
                {
                    await _serverHttpClient.SendMeasurementValueAsync(value).ConfigureAwait(true);
                }

                try
                {
                    await Task.Delay(1000, token).ConfigureAwait(true);
                }
                catch (TaskCanceledException)
                {
                    // Ignored
                }
            }
        }

        #region Interface Implementations

        /// <summary>
        ///     Gets the gateway data from the server.
        ///     You need the setupId to get detected by the server.
        /// </summary>
        /// <param name="id">The id to get detected by the server</param>
        /// <returns>The deserialized gateway from the server.</returns>
        public async Task<Gateway> GetGatewayDataAsync(string id)
        {
            HttpResponseMessage response = await _serverHttpClient.GetGatewayDataAsync(id).ConfigureAwait(true);
            string jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

            var t = BissDeserialize.FromJson<Gateway>(jsonString);

            return BissDeserialize.FromJson<Gateway>(jsonString);
        }

        public void Dispose()
        {
            _cts.Cancel();
            _cts.Dispose();
            _serverHttpClient.Dispose();
        }

        #endregion
    }
}