﻿// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       30.08.2021 15:55
// Developer:     Istvan Galfi
// Project:       DatabaseTables
// 
// Released under MIT

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables.ConfigurationTool
{
    /// <summary>
    ///     Table für  veröffentlichte Messungen.
    /// </summary>
    public class TablePublishedMeasurement
    {
        #region Properties

        /// <summary>
        ///     Der Id.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        /// <summary>
        ///     Der Id vom Projekt, dem dieses Instanz zugehört.
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        ///     Der Id vom User, dem dieses Instanz zugehört.
        /// </summary>
        public long? TblUserId { get; set; }

        /// <summary>
        ///     Der User, dem dieses Instanz zugehört.
        /// </summary>
        [ForeignKey(nameof(TblUserId))]
        public TableUser? TblUser { get; set; }

        /// <summary>
        ///     Der Name von diesem Instanz.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Die Beschreibung von diesem Instanz.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        ///     Der Port von diesem Instanz.
        /// </summary>
        public int Port { get; set; }

        #endregion
    }
}