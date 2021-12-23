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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Exchange.Model;
using Newtonsoft.Json;

namespace Database.Tables
{
    /// <summary>
    ///     <para>Device Tabelle für DB</para>
    ///     Klasse TableDevice. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class TableDevice : ExDevice
    {
        #region Properties

        /// <summary>
        ///     User der gerade oder als Letztes angemeldet war
        /// </summary>
        [ForeignKey(nameof(TblUserId))]
        [JsonIgnore]
        public virtual TableUser? TblUser { get; set; }

        /// <summary>
        ///     Geöffnete Views
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<TableViewState> ViewStates { get; set; } = new List<TableViewState>();
#pragma warning restore CA2227 // Collection properties should be read only

        #endregion
    }
}