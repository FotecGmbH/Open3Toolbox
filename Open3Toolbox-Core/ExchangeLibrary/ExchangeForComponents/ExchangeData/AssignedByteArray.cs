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

namespace ExchangeLibrary.ExchangeData
{
    /// <summary>
    ///     An assigned Byte array allows to shift a byte Array while holding an ID
    ///     Therefore you can identify the byte array
    /// </summary>
    /// <typeparam name="T">The type of the ID, eg long or GUID</typeparam>
    public class AssignedByteArray<T>
    {
        /// <summary>
        ///     The data, eg which is read from a stream
        /// </summary>
        public readonly byte[] Data;

        /// <summary>
        ///     The Id, eg from a sensor
        /// </summary>
        public readonly T Id;

        /// <summary>
        ///     Initializes a new instance of the assigned byte array
        /// </summary>
        /// <param name="id">The id</param>
        /// <param name="data">The data</param>
        public AssignedByteArray(T id, byte[] data)
        {
            Id = id;
            Data = data;
        }
    }
}