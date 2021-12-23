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
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace ExchangeLibrary.DataBaseData.Entities.Bases
{
    /// <summary>
    ///     Base class of gateway IEntity's
    /// </summary>
    public class TableBaseGateway
    {
        #region Properties

        /// <summary>
        ///     The Id of the gateway.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     The name of the gateway.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The description of the gateway.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The interval, in which the gateway should send its data to the server
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        ///     The id of the project this gateway is part of.
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        ///     The comunication how the gateway communicates
        ///     with the sensors
        /// </summary>
        public Comunication ComToSens { get; set; }

        /// <summary>
        ///     The Uri of the Server, the gateway may contact.
        /// </summary>
        public Uri ServerUrl { get; set; }

        #endregion
    }
}