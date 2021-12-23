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

namespace Database.Tables
{
    /// <summary>
    ///     <para>Files, Images etc</para>
    ///     Klasse TableFile. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    [Table("TblFile")]
    public class TableFile
    {
        #region Properties

        /// <summary>
        ///     DB Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        ///     Dateiname inkl. Endung
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///     Name im Blob
        /// </summary>
        public string BlobName { get; set; } = string.Empty;

        /// <summary>
        ///     Öffentlicher Link im CDN
        /// </summary>
        public string PublicLink { get; set; } = string.Empty;

        #region Profilbild User

        /// <summary>
        ///     User mit diesem Bild als Profilbild
        /// </summary>
        public virtual ICollection<TableUser> TblUserImages { get; set; } = new List<TableUser>();

        #endregion

        #endregion
    }
}