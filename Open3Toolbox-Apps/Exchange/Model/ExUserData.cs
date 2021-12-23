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
using Newtonsoft.Json;
using PropertyChanged;

namespace Exchange.Model
{
    /// <summary>
    ///     <para>Alle Daten eines Benutzers</para>
    ///     Klasse ExUserData. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExUserData : IBissModel
    {
        #region Properties

        /// <summary>
        ///     Datenbank Id des Benutzers
        /// </summary>
        [AlsoNotifyFor(nameof(UserOk))]
        public long Id { get; set; } = -1;

        /// <summary>
        ///     Benutzer ID gültig
        /// </summary>
        [JsonIgnore]
        public bool UserOk => Id > 0 && !string.IsNullOrEmpty(LoginName);

        /// <summary>
        ///     Passwort Hash des neuen User
        ///     Wird nur vom Client gesetzt und ist danach immer Leer!
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        ///     Name des eingeloggten User
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     Name des eingeloggten User
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        ///     Akzeptierte AGB version
        /// </summary>
        public string AgbVersion { get; set; } = string.Empty;

        /// <summary>
        ///     E-Mail
        /// </summary>
        public string UserEmail { get; set; } = string.Empty;

        /// <summary>
        ///     Login Name
        /// </summary>
        public string LoginName { get; set; } = string.Empty;

        /// <summary>
        ///     Standardsprache des Benutzers
        /// </summary>
        public string DefaultUserLanguage { get; set; } = "de";

        /// <summary>
        ///     Bild des Benutzers als Link
        /// </summary>
        public string UserImageLink { get; set; } = string.Empty;

        /// <summary>
        ///     Name des eingeloggten User
        /// </summary>
        [JsonIgnore]
        [DependsOn(nameof(FirstName), nameof(LastName))]
        public string FullName => $"{FirstName} {LastName}";

        #endregion

        #region Interface Implementations

#pragma warning disable CS0414
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged = null!;
#pragma warning restore CS0414

        #endregion
    }
}