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

using System.ComponentModel.DataAnnotations;

namespace Database.Tables
{
    /// <summary>
    ///     <para>TableSetting</para>
    ///     Klasse TableSetting. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class TableSetting
    {
        #region Properties

        /// <summary>
        ///     DB Id
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Aktuelle AGB Version - wenn User andere Version hat -> neu genehmigen notwendig
        /// </summary>
        public string AgbVersion { get; set; } = string.Empty;

        /// <summary>
        ///     Link zur aktuellen AGB Version
        /// </summary>
        public string AgbLink { get; set; } = string.Empty;

        /// <summary>
        ///     Version ab der eine Info an den User kommen soll, dass es ein Update gibt
        /// </summary>
        public string AppVersionInfo { get; set; } = string.Empty;

        /// <summary>
        ///     Version ab der die App nicht mehr nutzbar ist, und der User ein Update machen muss
        /// </summary>
        public string AppVersionError { get; set; } = string.Empty;

        /// <summary>
        ///     Wartungstext.
        /// </summary>
        public string MaintenanceText { get; set; } = string.Empty;

        /// <summary>
        ///     Wartung aktiv?
        /// </summary>
        public bool MaintenanceActive { get; set; }

        /// <summary>
        ///     Link zur Startumfrage.
        /// </summary>
        public string StartSurveyLink { get; set; } = string.Empty;

        /// <summary>
        ///     Link zur zweiten Umfrage nach 4 Wochen.
        /// </summary>
        public string SecondSurveyLink { get; set; } = string.Empty;

        /// <summary>
        ///     Link für iOS App
        /// </summary>
        public string AppcenterLinkIos { get; set; } = string.Empty;

        /// <summary>
        ///     Link für Android App
        /// </summary>
        public string AppcenterLinkDroid { get; set; } = string.Empty;

        #endregion
    }
}