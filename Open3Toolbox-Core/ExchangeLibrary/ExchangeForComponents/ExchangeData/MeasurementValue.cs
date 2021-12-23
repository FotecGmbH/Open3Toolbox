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

using Biss.Interfaces;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.DataBaseData.Interfaces.DTOs;

namespace ExchangeLibrary.ExchangeData
{
    using System;

    /// <summary>
    ///     The measurement value is the data which will be exchanged between
    ///     sensor, gateway and server and represents the data which is collected
    ///     from a single measurement
    /// </summary>
    [Serializable]
    public class MeasurementValue : IBissSerialize, IInputDTO<TableMeasurementData>, IOutputDTO<TableMeasurementData>
    {
        /// <summary>
        ///     Initializes a new instance of a measurement value.
        /// </summary>
        /// <param name="id">The id of the measurement, which collected the value</param>
        /// <param name="value">The value, which is collected from the measurement</param>
        /// <param name="timeStamp">The timestamp, when the data was collected</param>
        /// <param name="longitude">The longitude of the position, where the value was collected</param>
        /// <param name="latitude">The latitude of the position, where the value was collected</param>
        /// <param name="altitude">The altitude of the position, where the value was collected</param>
        public MeasurementValue(long id, double value, DateTime timeStamp, float longitude, float latitude, float altitude)
        {
            MeasurementId = id;
            Value = value;
            TimeStamp = timeStamp;
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
        }

        /// <summary>
        ///     Initializes a new instance of a measurement value.
        /// </summary>
        public MeasurementValue()
        {
        }

        #region Properties

        /// <summary>
        ///     The id of the measurement, which collected the value
        /// </summary>
        public long MeasurementId { get; set; }

        /// <summary>
        ///     The value, which is collected from the measurement
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        ///     The timestamp, when the data was collected
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///     The longitude of the position, where the value was collected
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        ///     The latitude of the position, where the value was collected
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        ///     The altitude of the position, where the value was collected
        /// </summary>
        public float Altitude { get; set; }

        #endregion
    }
}