// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       16.08.2021 08:24
// Developer:     Istvan Galfi
// Project:       WebServer
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biss.Dc.Core;
using Exchange.Model.ConfigurationTool;
using ExConfigExchange.Models;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>
    ///         Datenaustausch für <see cref="Exchange.Model.ConfigurationTool.Interfaces.IExConfigurable" /> und
    ///         <see cref="ExConfigExchange.Models.Interfaces.IExConfigItem" />s
    ///     </para>
    ///     Klasse ServerRemoteCalls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public partial class ServerRemoteCalls
    {
        #region Interface Implementations

        /// <summary>
        ///     Device fordert Listen Daten für ProjectTemplates
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>
        ///     Daten oder eine Exception auslösen
        /// </returns>
        public Task<List<DcServerListItem<ExProject>>> GetDcExProjectTemplates(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            long index = 0;
            long sortIndex = 0;
            var res = _configurableManager.GetProjectImplementations().Select(p => new DcServerListItem<ExProject> {Data = p, Index = ++index, SortIndex = ++sortIndex}).ToList();

            return Task.FromResult(res);
        }

        /// <summary>
        ///     Device will Listen Daten für ProjectTemplates sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <returns>
        ///     Ergebnis (bzw. Infos zum Fehler)
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<DcListStoreResult> StoreDcExProjectTemplates(long deviceId, long userId, List<DcStoreListItem<ExProject>> data, long secondId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Device fordert Listen Daten für GatewayTemplates
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>
        ///     Daten oder eine Exception auslösen
        /// </returns>
        public Task<List<DcServerListItem<ExGateway>>> GetDcExGatewayTemplates(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            long index = 0;
            long sortIndex = 0;
            var res = _configurableManager.GetGatewayImplementations().Select(g => new DcServerListItem<ExGateway> {Data = g, Index = ++index, SortIndex = ++sortIndex}).ToList();

            return Task.FromResult(res);
        }

        /// <summary>
        ///     Device will Listen Daten für GatewayTemplates sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <returns>
        ///     Ergebnis (bzw. Infos zum Fehler)
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<DcListStoreResult> StoreDcExGatewayTemplates(long deviceId, long userId, List<DcStoreListItem<ExGateway>> data, long secondId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Device fordert Listen Daten für SensorTemplates
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>
        ///     Daten oder eine Exception auslösen
        /// </returns>
        public Task<List<DcServerListItem<ExSensor>>> GetDcExSensorTemplates(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            long index = 0;
            long sortIndex = 0;
            var res = _configurableManager.GetSensorImplementations().Select(s => new DcServerListItem<ExSensor> {Data = s, Index = ++index, SortIndex = ++sortIndex}).ToList();

            return Task.FromResult(res);
        }

        /// <summary>
        ///     Device will Listen Daten für SensorTemplates sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <returns>
        ///     Ergebnis (bzw. Infos zum Fehler)
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<DcListStoreResult> StoreDcExSensorTemplates(long deviceId, long userId, List<DcStoreListItem<ExSensor>> data, long secondId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Device fordert Listen Daten für ImplementationTemplates2
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>
        ///     Daten oder eine Exception auslösen
        /// </returns>
        public Task<List<DcServerListItem<ExObjectConfigItem>>> GetDcExImplementationTemplates(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            long index = 0;
            long sortIndex = 0;
            var res = _configItemManager.GetTemplatesFor(filter).Select(c => new DcServerListItem<ExObjectConfigItem> {Data = c, Index = ++index, SortIndex = ++sortIndex}).ToList();

            return Task.FromResult(res);
        }

        /// <summary>
        ///     Device will Listen Daten für ImplementationTemplates2 sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <returns>
        ///     Ergebnis (bzw. Infos zum Fehler)
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<DcListStoreResult> StoreDcExImplementationTemplates(long deviceId, long userId, List<DcStoreListItem<ExObjectConfigItem>> data, long secondId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}