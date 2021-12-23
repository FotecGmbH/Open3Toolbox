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
    ///     Represents the bmp280 chip and implements the iopCodesChip interface
    /// </summary>
    public class BMP280 : IopCodesChip
    {
        /// <summary>
        ///     Configuration value for pressure 1
        /// </summary>
        private readonly Int64 _register_p1 = 37690;

        /// <summary>
        ///     Configuration value for pressure 2
        /// </summary>
        private readonly Int64 _register_p2 = -10463;

        /// <summary>
        ///     Configuration value for pressure 3
        /// </summary>
        private readonly Int64 _register_p3 = 3024;

        /// <summary>
        ///     Configuration value for pressure 4
        /// </summary>
        private readonly Int64 _register_p4 = 7612;

        /// <summary>
        ///     Configuration value for pressure 5
        /// </summary>
        private readonly Int64 _register_p5 = -21;

        /// <summary>
        ///     Configuration value for pressure 6
        /// </summary>
        private readonly Int64 _register_p6 = -7;

        /// <summary>
        ///     Configuration value for pressure 7
        /// </summary>
        private readonly Int64 _register_p7 = 15500;

        /// <summary>
        ///     Configuration value for pressure 8
        /// </summary>
        private readonly Int64 _register_p8 = -14600;

        /// <summary>
        ///     Configuration value for pressure 9
        /// </summary>
        private readonly Int64 _register_p9 = 6000;

        /// <summary>
        ///     Configuration value for temperature 1
        /// </summary>
        private readonly Int64 _register_t1 = 27504;

        /// <summary>
        ///     Configuration value for temperature 2
        /// </summary>
        private readonly Int64 _register_t2 = 26435;

        /// <summary>
        ///     Configuration value for temperature 3
        /// </summary>
        private readonly Int64 _register_t3 = -1000;

        #region Properties

        /// <summary>
        ///     The interfacetype eg "i2c" or "gpio"
        /// </summary>
        public InterfaceType InterfaceType { get; set; } = InterfaceType.i2CInterface;

        /// <summary>
        ///     The type of the chip
        /// </summary>
        public string ChipType { get; set; } = "bmp280";

        /// <summary>
        ///     Fullname of type
        /// </summary>
        public string TypeFullName { get; set; } = typeof(BMP280).FullName;

        /// <summary>
        ///     Fullname of assembly
        /// </summary>
        public string AssemblyFullname { get; set; } = Assembly.GetExecutingAssembly().GetName().Name;

        /// <summary>
        ///     How many bytes to read from the chip
        /// </summary>
        public int BytesToRead => 6;

        /// <summary>
        ///     How many bytes to write to the chip
        /// </summary>
        public int BytesToWrite => 0;

        #endregion

        /// <summary>
        ///     Parsing the readed bytes to the temperature value
        /// </summary>
        /// <param name="data">The readed bytes</param>
        /// <returns>The temperature value</returns>
        private (double, Int64) ReadTemp(byte[] data)
        {
            var bytes = data;

            Int64 value = bytes[0] << 16 | bytes[1] << 8 | bytes[2];

            var data64 = (Int32) (value);

            data64 >>= 4;

            var var1 = ((((data64 >> 3) - ((Int32) _register_t1 << 1))) * ((Int32) _register_t2)) >> 11;

            var var2 = (((((data64 >> 4) - ((Int32) _register_t1)) * ((data64 >> 4) - ((Int32) _register_t1))) >> 12) * ((Int32) _register_t3)) >> 14;

            var partRes = var1 + var2;

            double T = (partRes * 5 + 128) >> 8;

            T = (T / 100.0) - 3.6;

            if (T < 0)
            {
                T = T + 167.3;
            }

            //float degree = (T / 100) - 273;

            //double degree = ((T / 100.0) - 32.0) * 5.0 / 9.0;

            //double degree = (T - 32.0) * 5.0 / 9.0;

            return (T, partRes);
        }

        /// <summary>
        ///     Parsing the readed bytes to the temperature value
        /// </summary>
        /// <param name="data">The readed bytes</param>
        /// <param name="partRes">
        ///     A value from the temperature measuring which is needed for the pressure
        /// </param>
        /// <returns>The temperature value</returns>
        private double ReadPress(byte[] data, Int64 partRes)
        {
            var bytes = data; //BitConverter.GetBytes(data);
            double value = 0;

            //value = bytes[0] << 12 | bytes[1] << 4 | bytes[2] >> 4;
            var data64 = BitConverter.ToInt64(bytes);

            data64 >>= 4;

            var var1 = partRes - 128000;
            var var2 = var1 * var1 * _register_p6;
            var2 = var2 + (var1 * _register_p5 << 17);
            var2 = var2 + (_register_p4 << 35);
            var1 = ((var1 * var1 * _register_p3) >> 8) + ((var1 * _register_p2) << 12);
            var1 = (((((Int64) 1) << 47) + var1)) * (_register_p1) >> 33;

            if (var1 == 0)
            {
                return 0; // avoid exception caused by division by zero
            }

            var p = 1048576 - data64;
            p = (((p << 31) - var2) * 3125) / var1;
            var1 = (_register_p9 * (p >> 13) * (p >> 13)) >> 25;
            var2 = (_register_p8 * p) >> 19;

            p = ((p + var1 + var2) >> 8) + (_register_p7 << 4);
            return (float) p / 256;
        }

        #region Interface Implementations

        /// <summary>
        ///     Getting the read and write opcodes,
        ///     how to read and write to and from the chip.
        /// </summary>
        /// <param name="port">The port of the chip on which to read or write</param>
        /// <returns>The read and write opcodes</returns>
        public (byte[], byte[]) GetOpCodes(byte port)
        {
            var readOpCodes = new byte[0];

            if (port == 0)
            {
                readOpCodes = OpCodesDllHelper.I2CWrite(0x77, 0xF5, 0x00) // I2C.WRITE2(TO 0x77, REG 0xF5, BYTE 0x00)
                    .Concat(OpCodesDllHelper.I2CWrite(0x77, 0xF4, 37)) // I2C.WRITE2(TO 0x77, REG 0xF4, BYTE 37)
                    .Concat(new byte[] {10, 50}) // Delay(MS 50)                        // 6 only
                    .Concat(OpCodesDllHelper.I2CRead(0x77, 0xF7, 6)).ToArray(); // I2C.READ(FROM 0x77, REG 0xF7, 6) => SENDBUFFER
            }

            if (port == 1)
            {
                readOpCodes = OpCodesDllHelper.I2CWrite(0x77, 0xF5, 0x00) // I2C.WRITE2(TO 0x77, REG 0xF5, BYTE 0x00)
                    .Concat(OpCodesDllHelper.I2CWrite(0x77, 0xF4, 37)) // I2C.WRITE2(TO 0x77, REG 0xF4, BYTE 37)
                    .Concat(new byte[] {10, 50}) // Delay(MS 50)                        // 6 only
                    .Concat(OpCodesDllHelper.I2CRead(0x77, 0xFA, 6)).ToArray(); // I2C.READ(FROM 0x77, REG 0xFA, 6) => SENDBUFFER
            }


            byte[] writeOpCodes = { }; // BMP280 you cannot write

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
            double value = 0;

            if (port == 0) // Pressure
            {
                value = ReadPress(data, (ReadTemp(data)).Item2);
            }


            if (port == 1) // Temperature
            {
                value = ReadTemp(data).Item1;
            }


            return value;
        }

        /// <summary>
        ///     Parsing the input value to data, which can be written
        ///     to the chip, eg 20.3 degree to bytes
        /// </summary>
        /// <param name="data">The input value</param>
        /// <param name="port">The port of the chip</param>
        /// <returns>The value, which can be written to the chip</returns>
        public byte[] ParsingInputedValueToSendValue(double data, int port) => throw new ArgumentException("cannot Write to this Chip");

        /// <summary>
        ///     Getting the configurable options of the chip
        /// </summary>
        /// <returns>The configurable options</returns>
        public Dictionary<string, Type> GetConfigOptions() => throw new NotImplementedException();

        /// <summary>
        ///     Inputs for configuration of the chip
        /// </summary>
        /// <param name="inputs">The inputs</param>
        public void ConvertInputConfigOptions(Dictionary<string, object> inputs) => throw new NotImplementedException();

        #endregion
    }
}