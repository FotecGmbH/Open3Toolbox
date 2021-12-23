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
using System.IO;
using System.Linq;
using ExchangeLibrary.ConfigInterfaces;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using OpCodesDllsLibrary;

namespace ExchangeLibrary.Helper
{
    /// <summary>
    ///     This class represents a Helper for opcodes of a sensor.
    /// </summary>
    public static class SensorOpcodesHelper
    {
        /// <summary>
        ///     Adds all opcodes of the interfaces of the sensor to the sensor and
        ///     adds a delay and the other needed opcodes for running on the sensor
        /// </summary>
        /// <param name="sensor">The sensor on which to add the opcodes</param>
        public static void AddOpCodesTo(Sensor sensor)
        {
            if (sensor == null)
            {
                throw new ArgumentNullException(nameof(sensor));
            }

            sensor.AllOpCodes.Clear();

            List<IopCodesChip> opCodesChips = ChipDllsHandler.GetOpCodeChips(Directory.GetCurrentDirectory() + "\\bin\\Debug\\net5.0\\OpCodeDlls");
            InterfaceOpCodesVisitor visitor = new InterfaceOpCodesVisitor(opCodesChips);
            sensor.Interfaces.ToList().ForEach(interf => interf.Accept(visitor));
            var sensorOpCodes = visitor.OpCodes;

            sensorOpCodes.AddRange(OpCodesDllHelper.SoftDelay((ushort) sensor.MeasureInterval));
            sensorOpCodes.AddRange(new byte[] {12});
            sensorOpCodes.AddRange(new byte[] {63, sensor.MeasureXTimesTillSend});
            sensorOpCodes.AddRange(OpCodesDllHelper.ResetCounter());
            sensor.AllOpCodes = sensorOpCodes;
        }
    }
}