// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       BaseApp
// 
// Released under MIT

using System;
using System.Threading.Tasks;
using Biss.Dc.Client;
using Biss.Dc.Core;
using Biss.Interfaces;
using Biss.Serialize;
using Exchange.DataConnector;
using Exchange.Model;
using Exchange.Model.ConfigurationTool;
using Exchange.Model.Statistics;
using ExConfigExchange.Models;

namespace BaseApp.DataConnector
{
    /// <summary>
    ///     <para>Datenconnector für das aktuelle Projekt</para>
    ///     Klasse DcProjectBase. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class DcProjectBase : DcDataRoot
    {
        /// <summary>
        ///     Initializieren der neuen Instanz
        /// </summary>
        public DcProjectBase()
        {
            AutoConnect = false;
        }

        #region Properties

        /// <summary>
        ///     User Settings Daten
        /// </summary>
        public DcDataPoint<ExUser> DcExUserSettings { get; set; } = new DcDataPoint<ExUser>(EnumDcPointBehavior.LoadWhenNeeded, true, true);

        /// <summary>
        ///     Daten zur Wartung
        /// </summary>
        public DcDataPoint<ExMaintenance> DcExMaintenance { get; set; } = new DcDataPoint<ExMaintenance>(EnumDcPointBehavior.LoadWhenNeeded, true, cacheDataPoint: true);

        /// <summary>
        ///     Alle User
        /// </summary>
        public DcDataList<ExUser> DcExUsers { get; set; } = new DcDataList<ExUser>(false);

        /// <summary>
        ///     Infos vom Gerät, werden nur an den Server gesendet beim Start der App
        /// </summary>
        public DcDataPoint<ExDevice> DcExDevice { get; set; } = new DcDataPoint<ExDevice>(EnumDcPointBehavior.LocalOnly, cacheDataPoint: false, takeDefaultInstance: true);

        /// <summary>
        ///     Registrierung
        /// </summary>
        public DcDataPoint<ExUserData> DcExUserData { get; set; } = new DcDataPoint<ExUserData>(allowServerOverrideData: true, cacheDataPoint: false);

        /// <summary>
        ///     AppCenter Einstellungen
        /// </summary>
        public DcDataPoint<ExAppCenterSettings> DcAppCenterSettings { get; set; } = new DcDataPoint<ExAppCenterSettings>(allowServerOverrideData: true, cacheDataPoint: false);

        // Configuration Tool        
        /// <summary>
        ///     Projekt Implementation Vorlagen.
        /// </summary>
        public DcDataList<ExProject> DcExProjectTemplates { get; set; } = new DcDataList<ExProject>(false);

        /// <summary>
        ///     Gateway Implementation Vorlagen.
        /// </summary>
        public DcDataList<ExGateway> DcExGatewayTemplates { get; set; } = new DcDataList<ExGateway>(false);

        /// <summary>
        ///     Sensor Implementation Vorlagen.
        /// </summary>
        public DcDataList<ExSensor> DcExSensorTemplates { get; set; } = new DcDataList<ExSensor>(false);

        /// <summary>
        ///     Interface Implementation Vorlagen.
        /// </summary>
        public DcDataList<ExObjectConfigItem> DcExImplementationTemplates { get; set; } = new DcDataList<ExObjectConfigItem>(false);

        /// <summary>
        ///     UIds für die Pairing vom Gateways und Sensoren.
        /// </summary>
        public DcDataList<ExUid> DcExUids { get; set; } = new DcDataList<ExUid>(false);

        /// <summary>
        ///     Die Pairings vom Gateways/Sensoren und UIds.
        /// </summary>
        public DcDataList<ExPair> DcExPairs { get; set; } = new DcDataList<ExPair>(false);

        /// <summary>
        ///     Benutzer Projekte.
        /// </summary>
        public DcDataList<ExProject> DcExUserProjects { get; set; } = new DcDataList<ExProject>();

        /// <summary>
        ///     Benutzer Gateways.
        /// </summary>
        public DcDataList<ExGateway> DcExUserGateways { get; set; } = new DcDataList<ExGateway>();

        /// <summary>
        ///     Benutzer Sensoren.
        /// </summary>
        public DcDataList<ExSensor> DcExUserSensors { get; set; } = new DcDataList<ExSensor>();

        /// <summary>
        ///     Veröffentlichte Projekte
        /// </summary>
        public DcDataList<ExProject> DcExPublishedProjects { get; set; } = new DcDataList<ExProject>(false);

        // Statistik Sachen von Configuration Tool
        /// <summary>
        ///     Logische Unteransichten.
        /// </summary>
        public DcDataList<ExSubView> DcExSubViews { get; set; } = new DcDataList<ExSubView>();

        /// <summary>
        ///     Logische final Unteransichten.
        /// </summary>
        public DcDataList<ExFinalSubView> DcExFinalSubViews { get; set; } = new DcDataList<ExFinalSubView>();

        /// <summary>
        ///     Logische Aktor Ansichten.
        /// </summary>
        public DcDataList<ExActorView> DcExActorViews { get; set; } = new DcDataList<ExActorView>();

        /// <summary>
        ///     Logische Messung Ansichten.
        /// </summary>
        public DcDataList<ExMeasurementView> DcExMeasurementViews { get; set; } = new DcDataList<ExMeasurementView>();

        /// <summary>
        ///     Echte Aktor Details.
        /// </summary>
        public DcDataList<ExActorDetails> DcExActorDetails { get; set; } = new DcDataList<ExActorDetails>();

        /// <summary>
        ///     Echte Messung Details.
        /// </summary>
        public DcDataList<ExMeasurementDetails> DcExMeasurementDetails { get; set; } = new DcDataList<ExMeasurementDetails>();

        /// <summary>
        ///     Echte Messungsdaten.
        /// </summary>
        public DcDataList<ExMeasurementData> DcExMeasurementData { get; set; } = new DcDataList<ExMeasurementData>();

        #endregion


        /// <summary>
        ///     Daten Projektbezogen senden
        /// </summary>
        /// <param name="command"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<DcCommonCommandResult> Send(EnumDcCommonCommand command, string data)
        {
            return SendCommonData(new DcCommonData
                                  {
                                      Key = command.ToString(),
                                      Value = data
                                  });
        }

        /// <summary>
        ///     Daten Projektbezogen senden
        /// </summary>
        /// <param name="command"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public Task<DcCommonCommandResult> Send(EnumDcCommonCommand command, IBissSerialize data)
        {
            return SendCommonData(new DcCommonData
                                  {
                                      Key = command.ToString(),
                                      Value = data.ToJson()
                                  });
        }
    }
}