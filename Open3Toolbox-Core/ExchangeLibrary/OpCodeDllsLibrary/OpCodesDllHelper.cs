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

namespace OpCodesDllsLibrary
{
    /// <summary>
    ///     This represents a helper for opcodes
    ///     It creates byte arrays with the needed opcodes for specific operations
    /// </summary>
    public static class OpCodesDllHelper
    {
        /// <summary>
        ///     Writes a specific byte to a specific address via i2c
        /// </summary>
        /// <param name="address">The address of the chip</param>
        /// <param name="writeByte">The byte to write</param>
        /// <returns>The opcodes for this operation</returns>
        public static byte[] I2CWrite(byte address, byte writeByte) => new byte[] {25, address, writeByte};

        /// <summary>
        ///     Writes a specific byte to a specific register of the address of the chip via i2c
        /// </summary>
        /// <param name="address">The address of the chip</param>
        /// <param name="register">The register of the chip</param>
        /// <param name="writeByte">The byte to write</param>
        /// <returns>The opcodes for this operation</returns>
        public static byte[] I2CWrite(byte address, byte register, byte writeByte) => new byte[] {26, address, register, writeByte};

        /// <summary>
        ///     Reads how many bytes from a specific address via i2c
        /// </summary>
        /// <param name="address">The address of the chip</param>
        /// <param name="howManyBytes">How many bytes should be read</param>
        /// <returns>The opcodes for this operation</returns>
        public static byte[] I2CRead(byte address, byte howManyBytes) => new byte[] {20, address, howManyBytes};

        /// <summary>
        ///     Reads how many bytes from a specific register of the address of the chip via i2c
        /// </summary>
        /// <param name="address">The address of the chip</param>
        /// <param name="register">The register of the address of the chip</param>
        /// <param name="howManyBytes">How many bytes should be read</param>
        /// <returns>The opcodes for this operation</returns>
        public static byte[] I2CRead(byte address, byte register, byte howManyBytes) => new byte[] {21, address, register, howManyBytes};

        /// <summary>
        ///     Creates opcodes for a soft delay of milliseconds
        /// </summary>
        /// <param name="ms">The milliseconds to do the delay</param>
        /// <returns>The opcodes for this operation</returns>
        public static byte[] SoftDelay(ushort ms)
        {
            var lsb = (byte) (ms - (ms << 8));
            var msb = (byte) (ms >> 8);

            return new byte[] {9, msb, lsb};
        }

        /// <summary>
        ///     Sends the sendbuffer
        /// </summary>
        /// <returns>The opcodes for this operation</returns>
        public static byte[] SendBuffer() => new byte[] {60}; // 230 = log to serial

        /// <summary>
        ///     Resets the counter for the sendbuffer
        /// </summary>
        /// <returns>The opcodes for this operation</returns>
        public static byte[] ResetCounter() => new byte[] {11, 0, 0};
    }
}