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
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Threading;

namespace Exchange.Helper
{
    /// <summary>
    ///     The helper class for devices for eg serialports
    /// </summary>
    public static class DeviceHelper
    {
        /// <summary>
        ///     Detects the serial ports of the device
        /// </summary>
        /// <returns>The list of serial ports</returns>
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
                    string desc = item["Description"].ToString();
                    string deviceId = item["DeviceID"].ToString();
                    string name = item["Name"].ToString();

                    results.Add(deviceId + " " + desc + " " + name);
                }
            }
            catch (ManagementException e)
            {
            }

            results.Add("noCOM");

            return results;
        }

        /// <summary>
        ///     Writes bytes to a given serial port
        /// </summary>
        /// <param name="portName">Name of the port to write</param>
        /// <param name="bytesToSend">Bytes to send</param>
        public static void WriteToSerialPort(string portName, byte[] bytesToSend)
        {
            ////////////////////////// TEST /////////////////////////
            var firstLength = SerialPort.GetPortNames().Length;
            while (SerialPort.GetPortNames().Length < (firstLength + 1))
            {
                Thread.Sleep(10);
            }

            var g = SerialPort.GetPortNames();
            var g1 = g[g.Length - 1];
            SerialPort serialPort = new SerialPort();
            // Allow the user to set the appropriate properties.
            serialPort.PortName = g1;
            serialPort.ReadTimeout = 500;
            serialPort.WriteTimeout = 500;

            serialPort.Open();
            Thread.Sleep(100);

            var xy = bytesToSend.ToList();
            xy.Add(60);
            xy.Add(9);
            xy.Add(117);
            xy.Add(48);
            bytesToSend = xy.ToArray();
            serialPort.Write(bytesToSend, 0, bytesToSend.Length);

            serialPort.Close();
        }
    }
}