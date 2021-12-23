// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:49
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Collections.Generic;
using Biss.Interfaces;
using ExConfigExchange.JsonUtils;

namespace OpCodesDllsLibrary
{
    /// <summary>
    ///     A chip which implements this, is an opcodes chip so it describes for a chip
    ///     how to read and write to and from the chip with its opcodes
    /// </summary>
    public interface IopCodesChip : IBissSerialize, IDeserializable
    {
        #region Properties

        /// <summary>
        ///     The type of the chip, eg "bmp280"
        /// </summary>
        public string ChipType { get; set; }

        /// <summary>
        ///     The interfacetype eg "i2c" or "gpio"
        /// </summary>
        public InterfaceType InterfaceType { get; set; }

        /// <summary>
        ///     How many bytes to read from the chip
        /// </summary>
        public int BytesToRead { get; }

        /// <summary>
        ///     How many bytes to write to the chip
        /// </summary>
        public int BytesToWrite { get; }

        #endregion

        /// <summary>
        ///     Getting the read and write opcodes,
        ///     how to read and write to and from the chip.
        /// </summary>
        /// <param name="port">The port of the chip on which to read or write</param>
        /// <returns>The read and write opcodes</returns>
        public (byte[], byte[]) GetOpCodes(byte port);

        /// <summary>
        ///     Parsing the Value, which is readed from the Chip to a readable value
        ///     eg readed bytes to 20.4 Degree
        /// </summary>
        /// <param name="data">The readed value</param>
        /// <param name="port">The port from which the data came</param>
        /// <returns>The readable value</returns>
        public double ParsingReadedValueToValue(byte[] data, int port);

        /// <summary>
        ///     Parsing the input value to data, which can be written
        ///     to the chip, eg 20.3 degree to bytes
        /// </summary>
        /// <param name="data">The input value</param>
        /// <param name="port">The port of the chip</param>
        /// <returns>The value, which can be written to the chip</returns>
        public byte[] ParsingInputedValueToSendValue(double data, int port);

        /// <summary>
        ///     Inputs for configuration of the chip
        /// </summary>
        /// <param name="inputs">The inputs</param>
        public void ConvertInputConfigOptions(Dictionary<string, object> inputs);

        /// <summary>
        ///     Getting the configurable options of the chip
        /// </summary>
        /// <returns>The configurable options</returns>
        public Dictionary<string, Type> GetConfigOptions();
    }
}