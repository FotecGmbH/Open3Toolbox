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

namespace ExchangeLibrary.DataBaseData.Entities.Bases
{
    /// <summary>
    ///     Base class of sensor IEntity's
    /// </summary>
    public class TableBaseSensor
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        ///     The id of the sensor
        ///     for comparison, this is declared from the user
        /// </summary>
        public long SensorId { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        ///     The Jsonstring, which holds the whole data of the sensor
        /// </summary>
        public string JsonStringInfo { get; set; } = "123";

        /// <summary>
        ///     The interval, in which the gateway should send its data to the server
        /// </summary>
        public byte MeasureXTimesTillSend { get; set; }

        /// <summary>
        ///     The interval, in which the gateway should send its data to the server
        /// </summary>
        public int MeasureInterval { get; set; }

        /// <summary>
        ///     Gets or sets the gateway identifier.
        /// </summary>
        /// <value>
        ///     The id of the gateway, which contains this sensor.
        /// </value>
        public long GatewayId { get; set; }

        /// <summary>
        ///     Represents all opcodes which the sensor should work off
        /// </summary>
        public string AllOpCodes { get; set; } = "[26]"; // jsonformat 

        #endregion
    }
}