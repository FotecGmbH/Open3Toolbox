// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       10.11.2021 13:26
// Developer:     Matthias Mandl
// Project:       OpCodeDlls
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Reflection;
using OpCodesDllsLibrary;

namespace OpCodeDlls
{
    /// <summary>
    ///     Represents the gpioStandard chip and implements the iopCodesChip interface
    /// </summary>
    public class GpioStandard : IopCodesChip
    {
        #region Properties

        /// <summary>
        ///     The interfacetype eg "i2c" or "gpio"
        /// </summary>
        public InterfaceType InterfaceType { get; set; } = InterfaceType.gpioInterface;

        /// <summary>
        ///     The type of the chip
        /// </summary>
        public string ChipType { get; set; } = "gpiostandard";

        /// <summary>
        ///     Fullname of type
        /// </summary>
        public string TypeFullName { get; set; } = typeof(GpioStandard).FullName;

        /// <summary>
        ///     Fullname of assembly
        /// </summary>
        public string AssemblyFullname { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        ///     How many bytes to read from the chip
        /// </summary>
        public int BytesToRead => 1;

        /// <summary>
        ///     How many bytes to write to the chip
        /// </summary>
        public int BytesToWrite => 1;

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     Getting the read and write opcodes,
        ///     how to read and write to and from the chip.
        /// </summary>
        /// <param name="port">The port of the chip on which to read or write</param>
        /// <returns>The read and write opcodes</returns>
        public (byte[], byte[]) GetOpCodes(byte pin)
        {
            byte[] readOpCodes = {30, pin, 0, 32, pin};
            byte[] writeOpCodes = { };

            return (readOpCodes, writeOpCodes);
        }

        /// <summary>
        ///     Parsing the Value, which is readed from the Chip to a readable value
        ///     eg readed bytes to 20.4 Degree
        /// </summary>
        /// <param name="data">The readed value</param>
        /// <param name="port">The port from which the data came</param>
        /// <returns>The readable value</returns>
        public double ParsingReadedValueToValue(byte[] data, int port)
        {
            return data[0];
        }

        /// <summary>
        ///     Parsing the input value to data, which can be written
        ///     to the chip, eg 20.3 degree to bytes
        /// </summary>
        /// <param name="data">The input value</param>
        /// <param name="port">The port of the chip</param>
        /// <returns>The value, which can be written to the chip</returns>
        public byte[] ParsingInputedValueToSendValue(double data, int port)
        {
            var bytes = BitConverter.GetBytes(data);
            return bytes;
        }

        /// <summary>
        ///     Getting the configurable options of the chip
        /// </summary>
        /// <returns>The configurable options</returns>
        public Dictionary<string, Type> GetConfigOptions() => throw new NotImplementedException();

        /// <summary>
        ///     Inputs for configuration of the chip
        /// </summary>
        /// <param name="inputs">The inputs</param>
        public void ConvertInputConfigOptions(Dictionary<string, object> inputs)
        {
        }

        #endregion
    }
}