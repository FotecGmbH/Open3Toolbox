// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System;
using System.ComponentModel;
using Biss.Interfaces;

namespace Exchange.Model
{
    /// <summary>
    ///     <para>Daten für den FileTransfer</para>
    ///     Klasse ExFileUploadData. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExFileUploadData : IBissModel
    {
        #region Properties

        /// <summary>
        ///     DbId des Files.
        /// </summary>
        public long TblFileId { get; set; }

        /// <summary>
        ///     Id des dazugehörenden Infotextes.
        /// </summary>
        public long? TblInfoTextId { get; set; }

        /// <summary>
        ///     Id des dazugehörenden Erste Hilfe Themas.
        /// </summary>
        public long? TblFirstAidId { get; set; }

        /// <summary>
        ///     Bild (nur) löschen
        /// </summary>
        public bool DeleteImage { get; set; }

        #endregion

        #region Interface Implementations

#pragma warning disable 0067
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore 0067

        #endregion
    }
}