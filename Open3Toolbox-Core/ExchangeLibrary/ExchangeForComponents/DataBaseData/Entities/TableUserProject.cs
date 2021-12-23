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
    ///     The project for a user
    /// </summary>
    public class TableUserProject : TableBaseProject
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the company.
        /// </summary>
        /// <value>
        ///     The company, which owns this project.
        /// </value>
        public TableUserCompany Company { get; set; }

        /// <summary>
        ///     Gets or sets the gateways.
        /// </summary>
        /// <value>
        ///     The gateways, this project contains.
        /// </value>
        public ICollection<TableUserGateway> Gateways { get; set; }

        #endregion
    }
}