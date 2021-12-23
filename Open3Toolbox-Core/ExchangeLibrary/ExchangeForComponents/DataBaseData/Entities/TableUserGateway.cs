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
using ExchangeLibrary.DataBaseData.Entities.Bases;

namespace ExchangeLibrary.DataBaseData.Entities
{
    /// <summary>
    ///     The gateway for a user
    /// </summary>
    public class TableUserGateway : TableBaseGateway
    {
        #region Properties

        /// <summary>
        ///     The project this gateway is part of.
        /// </summary>
        public TableUserProject Project { get; set; }


        /// <summary>
        ///     The sensors, which are part of this gateway.
        /// </summary>
        public ICollection<TableUserSensor> Sensors { get; set; }

        #endregion
    }
}