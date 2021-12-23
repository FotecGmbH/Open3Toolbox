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
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Biss.Apps.Model;
using Biss.Collections;
using PropertyChanged;

namespace Exchange.Model
{
    /// <summary>
    ///     <para>ExUser</para>
    ///     Klasse ExUser. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ExUser : UserBase
    {
        #region Properties

        /// <summary>
        ///     Name des eingeloggten User
        /// </summary>
        [NotMapped]
        [JsonIgnore]
        [DependsOn(nameof(FirstName), nameof(LastName))]
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        ///     Ui Name für Fokus Zeit
        /// </summary>
        public string FocusTimeUiName { get; set; } = "Fokus/Projekt";

        /// <summary>
        ///     Vorname
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        ///     Nachname
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        ///     App ist mit DemoUser eingeloggt
        /// </summary>
        public bool IsDemoUser { get; set; }

        /// <summary>
        ///     Benutzer ist Administrator
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        ///     akzeptierte agb version
        /// </summary>
        public string AgbVersion { get; set; } = string.Empty;

        /// <summary>
        ///     Profilbild
        /// </summary>
        [NotMapped]
        public ExFile? UserImage { get; set; }

        /// <summary>
        ///     Wann der letzte Zugriff auf Account war.
        /// </summary>
        public DateTime LastAccessAtUtc { get; set; }

        /// <summary>
        ///     Abweichung zu UTC Zeit
        /// </summary>
        public TimeSpan TimeDifference { get; set; }

        /// <summary>
        ///     Geräte des Users
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public ObservableCollectionFilterable<ExDevice> Devices { get; set; } = new ObservableCollectionFilterable<ExDevice>();
#pragma warning restore CA2227 // Collection properties should be read only

        #endregion

        #region Settings

        /// <summary>
        ///     Ob Erste Hilfe aktiv.
        /// </summary>
        public bool FirstAidActive { get; set; }

        /// <summary>
        ///     Ob automatischer Start der Arbeitszeit.
        /// </summary>
        public bool AutoWorkStart { get; set; } = true;

        /// <summary>
        ///     Ob automatisches Ende der Arbeitszeit.
        /// </summary>
        public bool AutoWorkEnd { get; set; } = true;

        /// <summary>
        ///     $   Genauigkeit der Arbeitszeiterfassung.
        /// </summary>
        public TimeSpan Accuracy { get; set; } = new TimeSpan(0, 1, 0);

        /// <summary>
        ///     Ob Push Nachricht bei geplanten Arbeitszeitbeginn.
        /// </summary>
        public bool PushNotificationOnWorkStart { get; set; } = true;

        /// <summary>
        ///     Ob Push Nachricht bei geplanten Arbeitszeitende.
        /// </summary>
        public bool PushNotificationOnWorkEnd { get; set; } = true;

        /// <summary>
        ///     Ob Push Nachricht für Pausen.
        /// </summary>
        public bool PushNotificationOnPauseReminder { get; set; } = true;

        /// <summary>
        ///     Ob Push Nachricht bei Pausenende.
        /// </summary>
        public bool PushNotificationOnPauseEnd { get; set; } = true;

        /// <summary>
        ///     Ob Push Nachricht bei Ende konzentrierte Arbeit.
        /// </summary>
        public bool PushNotificationOnFocusEnd { get; set; } = true;

        /// <summary>
        ///     Ob Push Nachricht wenn in Freizeit gearbeitet wird.
        /// </summary>
        public bool PushNotificationOnWorkInFreetime { get; set; } = true;

        /// <summary>
        ///     Name der To-do-Liste in MS-To-do
        /// </summary>
        public string MsTodoListName { get; set; } = "Project";

        /// <summary>
        ///     Ob MS-To-do-Liste verwendet wird.
        /// </summary>
        public bool MsTodoListActive { get; set; } = true;

        /// <summary>
        ///     Ob erste Umfrage schon angeklickt wurde.
        /// </summary>
        public bool FirstSurveyLinkClicked { get; set; }

        /// <summary>
        ///     Ob zweite Umfrage schon angeklickt wurde.
        /// </summary>
        public bool SecondSurveyLinkClicked { get; set; }

        /// <summary>
        ///     Ob InfoTexte in der App angezeigt werden sollen.
        /// </summary>
        public bool ShowInfoTexts { get; set; }

        /// <summary>
        ///     Arbeitszeiten-Templates für alle Tage
        /// </summary>
        public bool WorkTemplatesAllDays { get; set; } = true;

        #endregion
    }
}