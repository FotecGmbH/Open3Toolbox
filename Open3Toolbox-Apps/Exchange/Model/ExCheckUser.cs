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

using System.ComponentModel;
using Biss.Interfaces;

namespace Exchange.Model
{
    /// <summary>
    ///     <para>ExCheckUser</para>
    ///     Klasse ExCheckUser. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExCheckUser : IBissModel
    {
        #region Properties

        /// <summary>
        ///     Datenbank Id des User
        /// </summary>
        public long UserId { get; set; } = -1;

        /// <summary>
        ///     Der User ist in der DB gesperrt und kann sich daher nicht anmelden
        /// </summary>
        public bool UserIsLocked { get; set; } = true;

        /// <summary>
        ///     Benutzer ist in der Db noch nicht verfügbar
        /// </summary>
        public bool IsNewUser { get; set; } = true;

        /// <summary>
        ///     Das aktuelle Gerät ist nicht (mehr) frei geschalten
        /// </summary>
        public bool? CurrentDeviceIsLocked { get; set; } = null!;

        /// <summary>
        ///     E-Mail wurde noch nicht validiert ob auch der Besitzer
        /// </summary>
        public bool EMailNotChecked { get; set; } = true;

        /// <summary>
        ///     Dieser Account ist der Demo Account - "Normales" Login ist nicht möglich!
        /// </summary>
        public bool IsDemoUser { get; set; }

        /// <summary>
        ///     Dieser Account muss die AGB akzeptieren.
        /// </summary>
        public bool NeedsAgb { get; set; }

        #endregion

        #region Interface Implementations

#pragma warning disable CS0414
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged = null!;
#pragma warning restore CS0414

        #endregion
    }
}