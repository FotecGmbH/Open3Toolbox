// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       DatabaseTables
// 
// Released under MIT

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Biss.Interfaces;

namespace Database.Tables
{
    /// <summary>
    ///     <para>ViewStatus für Auswertungen</para>
    ///     Klasse TableViewState. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    [Table("TableViewState")]
    public class TableViewState : IBissSerialize
    {
        #region Properties

        /// <summary>
        ///     DB Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     ViewName
        /// </summary>
        public string ViewName { get; set; } = string.Empty;

        /// <summary>
        ///     von
        /// </summary>
        public DateTime From { get; set; }

        /// <summary>
        ///     bis
        /// </summary>
        public DateTime To { get; set; }

        #endregion

        #region User

        /// <summary>
        ///     User der angemeldet ist
        /// </summary>
        public long? TblUserId { get; set; }

        /// <summary>
        ///     User der angemeldet ist
        /// </summary>
        [ForeignKey(nameof(TblUserId))]
        public virtual TableUser? TblUser { get; set; }

        #endregion

        #region Device

        /// <summary>
        ///     Device
        /// </summary>
        public long TblDeviceId { get; set; }

        /// <summary>
        ///     Device
        /// </summary>
        [ForeignKey(nameof(TblDeviceId))]
        public virtual TableDevice? TblDevice { get; set; }

        #endregion
    }
}