// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       30.08.2021 15:29
// Developer:     Matthias Mandl
// Project:       GatewayNetworkHandler
// 
// Released under MIT

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using ExchangeLibrary;
using ExchangeLibrary.ExchangeData;
using ExchangeLibrary.TTNCommunication;

namespace GatewayNetworkHandler
{
    /// <summary>
    ///     Represents the TTN communication to the sensors
    /// </summary>
    public class TtnCommunicationToSensors : IServerForSensors
    {
        #region Properties

        /// <summary>
        ///     All collected values from the sensor
        /// </summary>
        public ConcurrentQueue<MeasurementValue> Values { get; } = new ConcurrentQueue<MeasurementValue>();

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     Starts the Server listening to all incoming Connections and get the Data from them
        /// </summary>
        /// <returns>A Task</returns>
        public async Task StartAsync()
        {
            var client = new MqttClientTtn(new Command(obj =>
            {
                if (obj is Tuple<string, string> data)
                {
                    throw new NotImplementedException();
                    //Values.Add(...)
                }
            }));

            await client.StartAsync().ConfigureAwait(true);
        }

        #endregion
    }
}