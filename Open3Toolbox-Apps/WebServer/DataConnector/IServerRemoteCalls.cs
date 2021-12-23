// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
//
// Automatisch generierter Code. Nicht verändern!
// Erstellt am 11/23/2021 13:06:18

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Biss.Dc.Core;
using Biss.Dc.Server;
using Exchange.Model;
using Exchange.Model.ConfigurationTool;
using ExConfigExchange.Models;
using Exchange.Model.Statistics;

namespace WebServer.DataConnector
{
    /// <summary>
    /// Diese Funktionen müssen am Server implementiert werden
    /// </summary>
    public interface IServerRemoteCalls : IDcCoreRemoteCalls
    {
        #region DcExMaintenance         

        /// <summary>
        /// Device fordert Daten für DcExMaintenance
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<ExMaintenance> GetDcExMaintenance(long deviceId, long userId);

        /// <summary>
        /// Device will Daten für DcExNewUserData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcStoreResult> SetDcExMaintenance(long deviceId, long userId, ExMaintenance data);

        #endregion

        #region DcExUserData         

        /// <summary>
        /// Device fordert Daten für DcExUserData
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<ExUserData> GetDcExUserData(long deviceId, long userId);

        /// <summary>
        /// Device will Daten für DcExNewUserData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcStoreResult> SetDcExUserData(long deviceId, long userId, ExUserData data);

        #endregion

        #region DcAppCenterSettings         

        /// <summary>
        /// Device fordert Daten für DcAppCenterSettings
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<ExAppCenterSettings> GetDcAppCenterSettings(long deviceId, long userId);

        /// <summary>
        /// Device will Daten für DcExNewUserData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcStoreResult> SetDcAppCenterSettings(long deviceId, long userId, ExAppCenterSettings data);

        #endregion

        #region DcExProjectTemplates         

        /// <summary>
        /// Device fordert Listen Daten für DcExProjectTemplates
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExProject>>> GetDcExProjectTemplates(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExProjectTemplates sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExProjectTemplates(long deviceId, long userId, List<DcStoreListItem<ExProject>> data, long secondId);

        #endregion

        #region DcExGatewayTemplates         

        /// <summary>
        /// Device fordert Listen Daten für DcExGatewayTemplates
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExGateway>>> GetDcExGatewayTemplates(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExGatewayTemplates sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExGatewayTemplates(long deviceId, long userId, List<DcStoreListItem<ExGateway>> data, long secondId);

        #endregion

        #region DcExSensorTemplates         

        /// <summary>
        /// Device fordert Listen Daten für DcExSensorTemplates
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExSensor>>> GetDcExSensorTemplates(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExSensorTemplates sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExSensorTemplates(long deviceId, long userId, List<DcStoreListItem<ExSensor>> data, long secondId);

        #endregion

        #region DcExImplementationTemplates         

        /// <summary>
        /// Device fordert Listen Daten für DcExImplementationTemplates
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExObjectConfigItem>>> GetDcExImplementationTemplates(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExImplementationTemplates sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExImplementationTemplates(long deviceId, long userId, List<DcStoreListItem<ExObjectConfigItem>> data, long secondId);

        #endregion

        #region Uids         

        /// <summary>
        /// Device fordert Listen Daten für Uids
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExUid>>> GetDcExUids(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für Uids sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExUids(long deviceId, long userId, List<DcStoreListItem<ExUid>> data, long secondId);

        #endregion

        #region Pairs         

        /// <summary>
        /// Device fordert Listen Daten für Pairs
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExPair>>> GetDcExPairs(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für Pairs sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExPairs(long deviceId, long userId, List<DcStoreListItem<ExPair>> data, long secondId);

        #endregion

        #region DcExProjects         

        /// <summary>
        /// Device fordert Listen Daten für DcExProjects
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExProject>>> GetDcExUserProjects(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExProjects sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExUserProjects(long deviceId, long userId, List<DcStoreListItem<ExProject>> data, long secondId);

        #endregion

        #region DcExGateways         

        /// <summary>
        /// Device fordert Listen Daten für DcExGateways
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExGateway>>> GetDcExUserGateways(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExGateways sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExUserGateways(long deviceId, long userId, List<DcStoreListItem<ExGateway>> data, long secondId);

        #endregion

        #region DcExSensors         

        /// <summary>
        /// Device fordert Listen Daten für DcExSensors
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExSensor>>> GetDcExUserSensors(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExSensors sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExUserSensors(long deviceId, long userId, List<DcStoreListItem<ExSensor>> data, long secondId);

        #endregion

        #region DcExPublishedProjects         

        /// <summary>
        /// Device fordert Listen Daten für DcExPublishedProjects
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExProject>>> GetDcExPublishedProjects(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExPublishedProjects sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExPublishedProjects(long deviceId, long userId, List<DcStoreListItem<ExProject>> data, long secondId);

        #endregion

        #region DcExSubViews         

        /// <summary>
        /// Device fordert Listen Daten für DcExSubViews
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExSubView>>> GetDcExSubViews(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExSubViews sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExSubViews(long deviceId, long userId, List<DcStoreListItem<ExSubView>> data, long secondId);

        #endregion

        #region DcExFinalSubViews         

        /// <summary>
        /// Device fordert Listen Daten für DcExFinalSubViews
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExFinalSubView>>> GetDcExFinalSubViews(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExFinalSubViews sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExFinalSubViews(long deviceId, long userId, List<DcStoreListItem<ExFinalSubView>> data, long secondId);

        #endregion

        #region DcExActorViews         

        /// <summary>
        /// Device fordert Listen Daten für DcExActorViews
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExActorView>>> GetDcExActorViews(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExActorViews sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExActorViews(long deviceId, long userId, List<DcStoreListItem<ExActorView>> data, long secondId);

        #endregion

        #region DcExMeasurementViews         

        /// <summary>
        /// Device fordert Listen Daten für DcExMeasurementViews
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExMeasurementView>>> GetDcExMeasurementViews(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExMeasurementViews sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExMeasurementViews(long deviceId, long userId, List<DcStoreListItem<ExMeasurementView>> data, long secondId);

        #endregion

        #region DcExActorDetails         

        /// <summary>
        /// Device fordert Listen Daten für DcExActorDetails
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExActorDetails>>> GetDcExActorDetails(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExActorDetails sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExActorDetails(long deviceId, long userId, List<DcStoreListItem<ExActorDetails>> data, long secondId);

        #endregion

        #region DcExMeasurementDetails         

        /// <summary>
        /// Device fordert Listen Daten für DcExMeasurementDetails
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExMeasurementDetails>>> GetDcExMeasurementDetails(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExMeasurementDetails sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExMeasurementDetails(long deviceId, long userId, List<DcStoreListItem<ExMeasurementDetails>> data, long secondId);

        #endregion

        #region DcExMeasurementData         

        /// <summary>
        /// Device fordert Listen Daten für DcExMeasurementData
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        Task<List<DcServerListItem<ExMeasurementData>>> GetDcExMeasurementData(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter);

        /// <summary>
        /// Device will Listen Daten für DcExMeasurementData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        Task<DcListStoreResult> StoreDcExMeasurementData(long deviceId, long userId, List<DcStoreListItem<ExMeasurementData>> data, long secondId);

        #endregion

    }
}
