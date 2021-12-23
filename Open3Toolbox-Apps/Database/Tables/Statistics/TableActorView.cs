﻿// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       16.08.2021 08:24
// Developer:     Istvan Galfi
// Project:       DatabaseTables
// 
// Released under MIT

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables.Statistics
{
    /// <summary>
    ///     Table für Aktoransichten.
    /// </summary>
    [Table("TblActorView")]
    public class TableActorView
    {
        #region Properties

        /// <summary>
        ///     Der <see cref="ConfigurationTool.TablePublishedActor.Id" /> vom Aktor.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long ActorId { get; set; }

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
        ///     Der Id vom final Unteransicht, dem dieses Instanz zugehört.
        /// </summary>
        public long? FinalSubViewId { get; set; }

        /// <summary>
        ///     Der Id vom final Unteransicht, dem dieses Instanz zugehört.
        /// </summary>
        public TableFinalSubView? FinalSubView { get; set; }

        #endregion
    }
}