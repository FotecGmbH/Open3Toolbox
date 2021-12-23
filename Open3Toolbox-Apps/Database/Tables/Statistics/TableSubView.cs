// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       16.08.2021 08:24
// Developer:     Istvan Galfi
// Project:       DatabaseTables
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Tables.Statistics
{
    /// <summary>
    ///     Table für Unteransichten.
    /// </summary>
    [Table("TblSubView")]
    public class TableSubView
    {
        #region Properties

        /// <summary>
        ///     Der Id.
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        ///     <c>true</c> wenn teil des MainViews. (Entweder es ist <c>true</c> oder <see cref="SubViewId" /> ist)
        /// </summary>
        public bool IsPartOfMainView { get; set; }

        /// <summary>
        ///     Der Id vom Unteransicht, dem dieses Instanz zugehört. (Entweder es ist gesetzt oder <see cref="IsPartOfMainView" />
        ///     ist <c>true</c>)
        /// </summary>
        public long? SubViewId { get; set; }

        /// <summary>
        ///     Der Unteransicht, dem dieses Instanz zugehört.
        /// </summary>
        public TableSubView? SubView { get; set; }

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
        ///     Die Unteransichten die zu diesem Unteransicht gehören.
        /// </summary>
        [ForeignKey(nameof(SubViewId))]
        public ICollection<TableSubView> SubViews { get; set; } = new List<TableSubView>();

        /// <summary>
        ///     Die final Unteransichten die zu diesem Unteransicht gehören.
        /// </summary>
        [ForeignKey(nameof(TableFinalSubView.SubViewId))]
        public ICollection<TableFinalSubView> FinalSubViews { get; set; } = new List<TableFinalSubView>();

        #endregion
    }
}