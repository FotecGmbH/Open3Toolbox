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

using System.Diagnostics.CodeAnalysis;
using Biss.AppConfiguration;
using Biss.Log.Producer;
using Microsoft.Extensions.Logging;

namespace Exchange
{
    /// <summary>
    ///     <para>Konstanten für alle Projekte</para>
    ///     Klasse Constants. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    [SuppressMessage("Usage", "CA2211:Non-constant fields should not be visible", Justification = "<Pending>")]
    public static class Constants
    {
        /// <summary>
        ///     Benötigt die App Zugriff auf das Internet, bei false wird auch der empfang von Notifizierungen deaktiviert
        /// </summary>
        public const bool AppNeedInternet = true;

        /// <summary>
        ///     Besitz die App einen User (oder komplett ohne Userbezug)
        /// </summary>
        public const bool SupportLogin = true;

        /// <summary>
        ///     Gibt es einen Demo User
        /// </summary>
        public const bool HasDemoUser = true;

        /// <summary>
        ///     Aktuelle Appsettings
        /// </summary>
        public static AppSettings CurrentAppSettings = AppSettings.Current();

        /// <summary>
        ///     Titel für die Apps
        /// </summary>
        public static string MainTitle = "BISS Demo ";

        /// <summary>
        ///     Aktuelle App Settings für verschiedene Versionen (Release, Beta, Dev)
        /// </summary>
        public static AppConfigurationConstants AppConfiguration = new AppConfigurationConstants(AppSettings.Current().AppConfigurationConstants);


        /// <summary>
        ///     Konstanten für debuggen bzw testen
        /// </summary>
        static Constants()
        {
            Logging.Init(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace));
            //AppSettings.Current().DcSignalHost = "https://192.168.178.10:5002/";
        }

        #region Properties

        /// <summary>
        ///     DC Passwort bei Login mit MS
        /// </summary>
        public static string MsPassword { get; set; } = "string.Empty";

        /// <summary>
        ///     Link für AGB
        /// </summary>
        public static string AgbLink { get; set; } = "https://www.fhwn.ac.at/hochschule/institute/marktforschung-methodik/open3toolbox-nutzung-kontakt";

        #endregion
    }
}