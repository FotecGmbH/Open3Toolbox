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

namespace Exchange.Model
{
    /// <summary>
    ///     Necessary data for email
    /// </summary>
    public class ExNecessaryEmailData
    {
        #region Properties

        /// <summary>
        ///     The first name
        /// </summary>
        public string Firstname { get; set; } = string.Empty;

        /// <summary>
        ///     The last name
        /// </summary>
        public string Lastname { get; set; } = string.Empty;

        /// <summary>
        ///     The link
        /// </summary>
        public string Link { get; set; } = string.Empty;

        /// <summary>
        ///     The password
        /// </summary>
        public string Password { get; set; } = string.Empty;

        #endregion
    }
}