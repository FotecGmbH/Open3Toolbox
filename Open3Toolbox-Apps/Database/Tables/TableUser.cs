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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Exchange.Model;
using Newtonsoft.Json;

namespace Database.Tables
{
    /// <summary>
    ///     <para>TableUser</para>
    ///     Klasse TableUser. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class TableUser : ExUser
    {
        #region Properties

        /// <summary>
        ///     Token für UserExport
        /// </summary>
        public string ExportToken { get; set; } = string.Empty;

        /// <summary>
        ///     Microsoft Id
        /// </summary>
        public string MsId { get; set; } = string.Empty;

        /// <summary>
        ///     Geöffnete Views
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public virtual ICollection<TableViewState> ViewStates { get; set; } = new List<TableViewState>();
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        ///     UserProfilbild
        /// </summary>
        public virtual TableFile? TblUserImage { get; set; }

        /// <summary>
        ///     Geräte des Benutzers
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public ICollection<TableDevice> TblDevices { get; set; } = new List<TableDevice>();
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        ///     Userbild Datenbank Id
        /// </summary>
        [ForeignKey(nameof(TblUserImage))]
        [JsonIgnore]
        public long? TblUserImageId { get; set; }

        #endregion
    }
}