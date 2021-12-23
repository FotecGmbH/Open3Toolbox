// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       27.10.2021 08:50
// Developer:     Matthias Mandl
// Project:       GatewayNetworkHandler
// 
// Released under MIT

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using ExchangeLibrary.ExchangeData;
using ExchangeLibrary.Helper;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using ExchangeLibrary.Sensordata.Visitors;

namespace GatewayNetworkHandler
{
    /// <summary>
    ///     Represents the gateway for the sensors with udp communication
    /// </summary>
    public class UdpServerForSensors : IServerForSensors
    {
        /// <summary>
        ///     The locker
        /// </summary>
        private static object? _locker;

        /// <summary>
        ///     All sensors
        /// </summary>
        private readonly List<Sensor> _sensors;

        /// <summary>
        ///     Inititalize a new instance
        /// </summary>
        /// <param name="sensors">All sensors</param>
        /// <param name="locker">The locker</param>
        public UdpServerForSensors(List<Sensor> sensors, object locker)
        {
            _sensors = sensors ?? throw new ArgumentNullException(nameof(sensors));
            // _sensors.ForEach(sens=>_sensorsWhichNotSendetYet.Add(sens));
            _locker = locker ?? throw new ArgumentNullException(nameof(locker));
        }

        #region Properties

        /// <summary>
        ///     All collected values from the sensor
        /// </summary>
        public ConcurrentQueue<MeasurementValue> Values { get; } = new ConcurrentQueue<MeasurementValue>();

        /// <summary>
        ///     If the workers should exit
        /// </summary>
        public bool Exit { get; set; }

        /// <summary>
        ///     The currently available port names with the serial port class
        /// </summary>
        private Dictionary<string, SerialPort> CurrentPorts { get; } = new Dictionary<string, SerialPort>();

        /// <summary>
        ///     The currently hardware available ports
        /// </summary>
        private static List<string> AvailablePorts => DeviceHelper.DetectPorts();

        #endregion

        /// <summary>
        ///     A worker, which sends the configuration of all sensors to all
        ///     open/available ports
        /// </summary>
        /// <returns>A task</returns>
        private async Task SendToSensorsWorkerAsync()
        {
            while (!Exit)
            {
                lock (_locker!)
                {
                    foreach (var currentPort in CurrentPorts)
                    {
                        _sensors.ForEach(sen =>
                        {
                            var sendingData = new List<byte> {90, (byte) sen.SensorId};
                            sendingData.AddRange(sen.AllOpCodes);
                            sendingData.Add(90);

                            try
                            {
                                currentPort.Value.Write(sendingData.ToArray(), 0, sendingData.Count);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("could not write to serial port: " + currentPort.Key + " at " + DateTime.Now.ToLongTimeString());
                                // Serial Port is often not available for a short amount of time
                            }
                        });
                    }
                }

                await Task.Delay(5000).ConfigureAwait(true);
            }
        }

        /// <summary>
        ///     Refreshes the port, if some ports need to be deleted or new ports to get initialized
        /// </summary>
        private void RefreshPorts()
        {
            lock (_locker!)
            {
                List<string> newPorts = AvailablePorts.Where(aP => !CurrentPorts.ContainsKey(aP)).ToList();
                List<SerialPort> deletePorts = CurrentPorts.Where(cP => !AvailablePorts.Contains(cP.Key)).Select(cP => cP.Value).ToList();

                DeviceHelper.DisposePorts(deletePorts);

                Dictionary<string, SerialPort> initaInitializedPorts = DeviceHelper.InitializePorts(newPorts);
                foreach (var (key, value) in initaInitializedPorts)
                {
                    CurrentPorts.Add(key, value);
                }
            }
        }

        /// <summary>
        ///     A worker, which reads every of the current ports,
        ///     if a sensor wants to send data and interprets the data
        /// </summary>
        /// <returns>A task</returns>
        private async Task GetDataFromSensorsWorkerAsync()
        {
            using var udpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 1234));
            while (!Exit)
            {
                var receivedResults = await udpClient.ReceiveAsync().ConfigureAwait(true);
                var data = receivedResults.Buffer;

                if (data.Length > 0)
                {
                    byte id = id = data[0];
                    data = data.Skip(1).ToArray();

                    if (data.Length == 0 || _sensors.All(sens => sens.SensorId != id))
                    {
                        continue;
                    }

                    var curr = _sensors.First(sens => sens.SensorId == id);

                    Console.WriteLine("Got data from sensor with sensorId: " + curr.SensorId + " with length " + data.Length + " at " + DateTime.Now.ToLongTimeString());

                    var visitor = new InterfaceValuesVisitor(data, _locker!);

                    for (var h = 0; h < curr.MeasureXTimesTillSend; h++)
                    {
                        curr.Interfaces.ForEach(interf => interf.Accept(visitor));
                    }

                    foreach (var value in visitor.Values)
                    {
                        Values.Enqueue(value);
                    }
                }

                await Task.Delay(400).ConfigureAwait(true);
            }
        }

        #region Interface Implementations

        /// <summary>
        ///     Starts the Server listening to all incoming Connections and get the Data from them
        /// </summary>
        /// <returns>A Task</returns>
        public async Task StartAsync()
        {
            RefreshPorts();
            await ((Task) Task.Factory.StartNew(async () => await SendToSensorsWorkerAsync().ConfigureAwait(true))).ConfigureAwait(true);

            await Task.Factory.StartNew(async () => await GetDataFromSensorsWorkerAsync().ConfigureAwait(true)).ConfigureAwait(true);

            while (!Exit)
            {
                RefreshPorts();
                await Task.Delay(400).ConfigureAwait(true);
                if (Console.KeyAvailable)
                {
                    Exit = true;
                }
            }

            DeviceHelper.DisposePorts(CurrentPorts.Values.ToList());
        }

        #endregion
    }
}