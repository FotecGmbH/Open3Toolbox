// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Management;
using System.Threading;

namespace ExchangeLibrary.Helper
{
    /// <summary>
    ///     Represents a helper for getting devices via serial ports and writing to them
    /// </summary>
    public static class DeviceHelper
    {
        /// <summary>
        ///     Detect serial attached devices
        /// </summary>
        /// <returns>List of all connected ports</returns>
        public static List<string> DetectPorts()
        {
            ManagementScope connectionScope = new ManagementScope();
            SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);
            List<string> results = new List<string>();
            try
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string deviceId = item["DeviceID"].ToString();

                    results.Add(deviceId);
                }
            }
            catch (ManagementException)
            {
            }

            return results;
        }

        /// <summary>
        ///     Initializes ports, so they can be used for comunicating
        /// </summary>
        /// <param name="newPorts">The list of names of ports, that should be initialized</param>
        /// <returns>A dictionary with the names and their serialport</returns>
        public static Dictionary<string, SerialPort> InitializePorts(List<string> newPorts)
        {
            if (newPorts == null)
            {
                throw new ArgumentNullException(nameof(newPorts));
            }

            Dictionary<string, SerialPort> result = new Dictionary<string, SerialPort>();

            newPorts.ForEach(nP =>
            {
                SerialPort serialPort = new SerialPort
                                        {
                                            BaudRate = 9600,
                                            PortName = nP,
                                            ReadTimeout = 500,
                                            WriteTimeout = 500
                                        };

                serialPort.Open();
                result.Add(nP, serialPort);
            });

            return result;
        }

        /// <summary>
        ///     Disposes a list of serial ports
        /// </summary>
        /// <param name="deletePorts">The ports that should be disposed</param>
        public static void DisposePorts(List<SerialPort> deletePorts)
        {
            if (deletePorts == null)
            {
                throw new ArgumentNullException(nameof(deletePorts));
            }

            deletePorts.ForEach(deletePort => deletePort.Close());
        }


        /// <summary>
        ///     Writes the given bytes to the given port via serial communication
        /// </summary>
        /// <param name="portName">The portname where to send</param>
        /// <param name="bytesToSend">The bytes to send</param>
        public static void WriteToSerialPort(string portName, byte[] bytesToSend) // eg COM1
        {
            if (portName == null)
            {
                throw new ArgumentNullException(nameof(portName));
            }

            if (bytesToSend == null)
            {
                throw new ArgumentNullException(nameof(bytesToSend));
            }

            portName = portName[..4];
            SerialPort serialPort = new SerialPort
                                    {
                                        PortName = portName,

                                        ReadTimeout = 500,
                                        WriteTimeout = 500
                                    };

            serialPort.Open();
            serialPort.Write(bytesToSend, 0, bytesToSend.Length);
            serialPort.Close();
        }


        /// <summary>
        ///     Read all bytes from the given port
        /// </summary>
        /// <param name="portName">The given portname to send</param>
        /// <returns>The readed bytes</returns>
        public static byte[] ReadFromSerialPort(string portName)
        {
            portName = portName[..4];
            SerialPort serialPort = new SerialPort {PortName = portName, ReadTimeout = 500, WriteTimeout = 500, BaudRate = 9600};

            serialPort.Open();
            byte[] result = new byte[serialPort.BytesToRead];
            if (serialPort.BytesToRead > 1)
            {
                Thread.Sleep(2000);
                result = new byte[serialPort.BytesToRead];
                serialPort.Read(result, 0, serialPort.BytesToRead);
            }

            serialPort.Close();
            return result;
        }
    }
}