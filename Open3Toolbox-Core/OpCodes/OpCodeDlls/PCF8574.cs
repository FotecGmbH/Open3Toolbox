// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       08.11.2021 14:01
// Developer:     Matthias Mandl
// Project:       OpCodeDlls
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpCodesDllsLibrary;

namespace OpCodeDlls
{
    /// <summary>
    ///     Represents the pcf8574 chip and implements the iopCodesChip interface
    /// </summary>
    public class PCF8574 : IopCodesChip
    {
        /// <summary>
        ///     Conversion from input/output to byte
        /// </summary>
        private readonly Dictionary<int, byte> _pinValuePairs = new Dictionary<int, byte>
                                                                {
                                                                    // User enters port 0-7, but reading data from the chip returns 0-255, so it must be converted
                                                                    {0, 1},
                                                                    {1, 2},
                                                                    {2, 4},
                                                                    {3, 8},
                                                                    {4, 16},
                                                                    {5, 32},
                                                                    {6, 64},
                                                                    {7, 128}
                                                                };

        #region Properties

        /// <summary>
        ///     The interfacetype eg "i2c" or "gpio"
        /// </summary>
        public InterfaceType InterfaceType { get; set; } = InterfaceType.i2CInterface;

        /// <summary>
        ///     The type of the chip
        /// </summary>
        public string ChipType { get; set; } = "pcf8574";

        /// <summary>
        ///     Fullname of type
        /// </summary>
        public string TypeFullName { get; set; } = typeof(PCF8574).FullName;

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
        public (byte[], byte[]) GetOpCodes(byte port)
        {
            var readOpCodes = OpCodesDllHelper.I2CWrite(0x20, 0xff) // I2C.WRITE2(TO 0x77, REG 0xF5, BYTE 0x00)
                .Concat(OpCodesDllHelper.I2CRead(0x20, 1)).ToArray(); // I2C.READ(FROM 0x77, REG 0xF7, 6) => SENDBUFFER

            byte[] writeOpCodes = { }; // not implemented yet

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
            if (port < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(port));
            }

            if (data < 0 || data > 255)
            {
                throw new ArgumentOutOfRangeException(nameof(port));
            }

            return new[] {_pinValuePairs[port]};
        }

        /// <summary>
        ///     Inputs for configuration of the chip
        /// </summary>
        /// <param name="inputs">The inputs</param>
        public void ConvertInputConfigOptions(Dictionary<string, object> inputs) => throw new NotImplementedException();

        /// <summary>
        ///     Getting the configurable options of the chip
        /// </summary>
        /// <returns>The configurable options</returns>
        public Dictionary<string, Type> GetConfigOptions() => throw new NotImplementedException();

        #endregion
    }
}