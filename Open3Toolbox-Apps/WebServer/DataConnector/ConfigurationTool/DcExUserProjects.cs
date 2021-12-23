// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       30.08.2021 15:55
// Developer:     Istvan Galfi
// Project:       WebServer
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biss.Dc.Core;
using Biss.Log.Producer;
using Biss.Serialize;
using Database.Context;
using Exchange.Model.ConfigurationTool;
using Exchange.Services.ConfigurationTool;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.Helper;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WebExchange;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Datenaustausch für die Benutzer Projekte (Die noch konfiguriert werden.).</para>
    ///     Klasse ServerRemoteCalls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    /// <seealso cref="WebServer.DataConnector.ServerRemoteCallBase" />
    /// <seealso cref="WebServer.DataConnector.IServerRemoteCalls" />
    public partial class ServerRemoteCalls
    {
        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Project Map(TableUserProject project)
        {
            return new Project
                   {
                       Id = project.Id,
                       Company = Map(project.Company),
                       Description = project.Description,
                       Name = project.Name,
                       Gateways = project.Gateways.Select(g => Map(g)).ToList()
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="company">Die Firma.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Company Map(TableUserCompany company)
        {
            return new Company
                   {
                       Id = company.Id,
                       Email = company.Email,
                       Name = company.Name,
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="gateway">Der Gateway.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Gateway Map(TableUserGateway gateway)
        {
            return new Gateway
                   {
                       Id = gateway.Id,
                       Name = gateway.Name,
                       Description = gateway.Description,
                       Interval = gateway.Interval,
                       ServerUrl = gateway.ServerUrl,
                       ComToSens = gateway.ComToSens,
                       Sensors = gateway.Sensors.Select(s => Map(s)).ToList(),
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="sensor">Der Sensor.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Sensor Map(TableUserSensor sensor)
        {
            return new Sensor
                   {
                       SensorId = sensor.Id,
                       Name = sensor.Name,
                       Description = sensor.Description,
                       MeasureXTimesTillSend = sensor.MeasureXTimesTillSend,
                       MeasureInterval = sensor.MeasureInterval,
                       Interfaces = BissDeserialize.FromJson<List<CommunicationInterface>>(sensor.JsonStringInfo),
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        /// <param name="id">Der Id vom Projekt.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Project Map(ExProject project, long id)
        {
            return BissDeserialize.FromJson<Project>(ExConfigurableJsonConverter.ToJSONNoChildren(id, project));
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="gateway">Der Gateway.</param>
        /// <param name="id">Der Id vom Gateway.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Gateway Map(ExGateway gateway, long id)
        {
            var json = ExConfigurableJsonConverter.ToJSONNoChildren(id, gateway);
            return BissDeserialize.FromJson<Gateway>(json);
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="sensor">Der Sensor.</param>
        /// <param name="id">Der Id vom Sensor.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Sensor Map(ExSensor sensor, long id)
        {
            return BissDeserialize.FromJson<Sensor>(ExConfigurableJsonConverter.ToJSON(id, sensor));
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TableUserProject Map(Project project, long userId)
        {
            return new TableUserProject
                   {
                       Id = project.Id < 0 ? 0 : project.Id,
                       Company = Map(project.Company, userId),
                       Description = project.Description,
                       Name = project.Name,
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="company">Die Firma.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TableUserCompany Map(Company company, long userId)
        {
            return new TableUserCompany
                   {
                       Id = company.Id < 0 ? 0 : company.Id,
                       Email = company.Email,
                       Name = company.Name,
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="gateway">Der Gateway.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="projectId">Der Projekt Id, dem der Gateway zugehört.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TableUserGateway Map(Gateway gateway, long userId, long projectId)
        {
            return new TableUserGateway
                   {
                       ProjectId = projectId,
                       Id = gateway.Id < 0 ? 0 : gateway.Id,
                       Name = gateway.Name,
                       Description = gateway.Description,
                       Interval = gateway.Interval,
                       ServerUrl = gateway.ServerUrl,
                       ComToSens = gateway.ComToSens
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="sensor">Der Sensor.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="gatewayId">Der Gateway Id, dem der Sensor zugehört.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TableUserSensor Map(Sensor sensor, long userId, long gatewayId)
        {
            SensorOpcodesHelper.AddOpCodesTo(sensor);

            return new TableUserSensor
                   {
                       GatewayId = gatewayId,
                       Id = sensor.Id < 0 ? 0 : sensor.Id,
                       SensorId = sensor.SensorId,
                       Name = sensor.Name,
                       Description = sensor.Description,
                       MeasureInterval = sensor.MeasureInterval,
                       MeasureXTimesTillSend = sensor.MeasureXTimesTillSend,
                       JsonStringInfo = sensor.Interfaces.ToJson(),
                       AllOpCodes = sensor.AllOpCodes.ToJson()
                   };
        }

        #region Interface Implementations

        /// <summary>
        ///     Device fordert Listen Daten für DcExProjects
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
        public Task<List<DcServerListItem<ExProject>>> GetDcExUserProjects(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            var newUserId = userId == -1 ? 1 : userId;
            if (_sensorCacher.IsUserKnown(newUserId))
            {
                return Task.FromResult(_projectCacher.GetCachedItems(newUserId).Select(p => new DcServerListItem<ExProject> {Data = p.Value, Index = p.Key, SortIndex = p.Key}).ToList());
            }

            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var projectTablesQuery = db.TblUserProjects
                .Include(p => p.Company)
                .Include(p => p.Gateways).ThenInclude(g => g.Sensors)
                .Where(p => p.Id > 0);

            var toReturn = CacheProjectsWithChildren(newUserId, projectTablesQuery.Select(p => Map(p)));

            return Task.FromResult(toReturn.Select(p => new DcServerListItem<ExProject> {Data = p.Value, Index = p.Key, SortIndex = p.Key}).AsParallel().ToList());
        }

        /// <summary>
        ///     Device will Listen Daten für DcExProjects sichern
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
        public async Task<DcListStoreResult> StoreDcExUserProjects(long deviceId, long userId, List<DcStoreListItem<ExProject>> data, long secondId)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var result = new DcListStoreResult {ElementsStored = 0, NewIndex = new List<DcListStoreResultIndex>(), StoreResult = new DcStoreResult {ErrorType = EnumServerError.None, ServerExceptionText = ""}};
            var indexedEntities = new Dictionary<long, EntityEntry<TableUserProject>>(); // Old index to entity
            var newProjects = new List<Tuple<EntityEntry<TableUserProject>, ExProject>>();
            foreach (var item in data)
            {
                var mapped = Map(Map(item.Data, item.Index), newUserId);
                switch (item.State)
                {
                    case EnumDcListElementState.New:
                        var addEntity = db.TblUserProjects.Add(mapped);
                        indexedEntities.Add(item.Index, addEntity);
                        newProjects.Add(new Tuple<EntityEntry<TableUserProject>, ExProject>(addEntity, item.Data));
                        break;
                    case EnumDcListElementState.Modified:
                        var updateEntity = db.TblUserProjects.Update(mapped);
                        indexedEntities.Add(item.Index, updateEntity);
                        _projectCacher.DeleteCachedItem(newUserId, item.Index);
                        newProjects.Add(new Tuple<EntityEntry<TableUserProject>, ExProject>(updateEntity, item.Data));
                        break;
                    case EnumDcListElementState.Deleted:
                        db.TblUserProjects.Remove(mapped);
                        _projectCacher.DeleteCachedItem(newUserId, item.Index);
                        break;
                    case EnumDcListElementState.None:
                        continue;
                }

                result.ElementsStored++;
            }

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logging.Log.LogError(e.Message);
                return new DcListStoreResult {StoreResult = new DcStoreResult {ErrorType = EnumServerError.ServerException, ServerExceptionText = e.Message}};
            }

            foreach (var oldIndexEntityPair in indexedEntities)
            {
                result.NewIndex.Add(new DcListStoreResultIndex {BeforeStoreIndex = oldIndexEntityPair.Key, NewIndex = oldIndexEntityPair.Value.Entity.Id});
            }

            foreach (var added in newProjects)
            {
                _projectCacher.CacheItem(newUserId, added.Item1.Entity.Id, added.Item2);
            }

            return result;
        }

        /// <summary>
        ///     Device fordert Listen Daten für DcExGateways
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
        public Task<List<DcServerListItem<ExGateway>>> GetDcExUserGateways(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            var newUserId = userId == -1 ? 1 : userId;
            if (_sensorCacher.IsUserKnown(newUserId))
            {
                return Task.FromResult(_gatewayCacher.GetCachedItems(newUserId).Select(g => new DcServerListItem<ExGateway> {Data = g.Value, Index = g.Key, SortIndex = g.Key}).ToList());
            }

            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var projectTablesQuery = db.TblUserProjects
                .Include(p => p.Gateways).ThenInclude(g => g.Sensors)
                .Where(p => p.Id > 0);

            var toReturn = projectTablesQuery.Select(p => CacheGatewaysWithChildren(newUserId, p.Id, p.Gateways.Select(g => Map(g)))).AsEnumerable().SelectMany(x => x);

            return Task.FromResult(toReturn.Select(g => new DcServerListItem<ExGateway> {Data = g.Value, Index = g.Key, SortIndex = g.Key}).AsParallel().ToList());
        }

        /// <summary>
        ///     Device will Listen Daten für DcExGateways sichern
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
        public async Task<DcListStoreResult> StoreDcExUserGateways(long deviceId, long userId, List<DcStoreListItem<ExGateway>> data, long secondId)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var result = new DcListStoreResult {ElementsStored = 0, NewIndex = new List<DcListStoreResultIndex>(), StoreResult = new DcStoreResult {ErrorType = EnumServerError.None, ServerExceptionText = ""}};
            var indexedEntities = new Dictionary<long, EntityEntry<TableUserGateway>>(); // Old index to entity
            var newGateways = new List<Tuple<EntityEntry<TableUserGateway>, ExGateway>>();
            foreach (var item in data)
            {
                var mapped = Map(Map(item.Data, item.Index), newUserId, item.Data.ProjectId); // Where the Gateway belongs must be mapped as well.
                switch (item.State)
                {
                    case EnumDcListElementState.New:
                        var addEntity = db.TblUserGateways.Add(mapped);
                        indexedEntities.Add(item.Index, addEntity);
                        newGateways.Add(new Tuple<EntityEntry<TableUserGateway>, ExGateway>(addEntity, item.Data));
                        break;
                    case EnumDcListElementState.Modified:
                        var updateEntity = db.TblUserGateways.Update(mapped);
                        indexedEntities.Add(item.Index, updateEntity);
                        _gatewayCacher.DeleteCachedItem(newUserId, item.Index);
                        newGateways.Add(new Tuple<EntityEntry<TableUserGateway>, ExGateway>(updateEntity, item.Data));
                        break;
                    case EnumDcListElementState.Deleted:
                        db.TblUserGateways.Remove(mapped);
                        _gatewayCacher.DeleteCachedItem(newUserId, item.Index);
                        break;
                    case EnumDcListElementState.None:
                        continue;
                }

                result.ElementsStored++;
            }

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logging.Log.LogError(e.Message);
                return new DcListStoreResult {StoreResult = new DcStoreResult {ErrorType = EnumServerError.ServerException, ServerExceptionText = e.Message}};
            }

            foreach (var oldIndexEntityPair in indexedEntities)
            {
                result.NewIndex.Add(new DcListStoreResultIndex {BeforeStoreIndex = oldIndexEntityPair.Key, NewIndex = oldIndexEntityPair.Value.Entity.Id});
            }

            foreach (var added in newGateways)
            {
                _gatewayCacher.CacheItem(newUserId, added.Item1.Entity.Id, added.Item2);
            }

            return result;
        }

        /// <summary>
        ///     Device fordert Listen Daten für DcExSensors
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
        public Task<List<DcServerListItem<ExSensor>>> GetDcExUserSensors(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            var newUserId = userId == -1 ? 1 : userId;
            if (_sensorCacher.IsUserKnown(newUserId))
            {
                return Task.FromResult(_sensorCacher.GetCachedItems(newUserId).Select(s => new DcServerListItem<ExSensor> {Data = s.Value, Index = s.Key, SortIndex = s.Key}).ToList());
            }

            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var gatewayTablesQuery = db.TblUserGateways
                .Include(g => g.Sensors)
                .Where(p => p.Id > 0);

            var toReturn = gatewayTablesQuery.Select(g => CacheSensors(newUserId, g.Id, g.Sensors.Select(s => Map(s)))).AsEnumerable().SelectMany(x => x);

            return Task.FromResult(toReturn.Select(s => new DcServerListItem<ExSensor> {Data = s.Value, Index = s.Key, SortIndex = s.Key}).AsParallel().ToList());
        }

        /// <summary>
        ///     Device will Listen Daten für DcExSensors sichern
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
        public async Task<DcListStoreResult> StoreDcExUserSensors(long deviceId, long userId, List<DcStoreListItem<ExSensor>> data, long secondId)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var result = new DcListStoreResult {ElementsStored = 0, NewIndex = new List<DcListStoreResultIndex>(), StoreResult = new DcStoreResult {ErrorType = EnumServerError.None, ServerExceptionText = ""}};
            var indexedEntities = new Dictionary<long, EntityEntry<TableUserSensor>>(); // Old index to entity
            var newSensors = new List<Tuple<EntityEntry<TableUserSensor>, ExSensor>>();
            foreach (var item in data)
            {
                var mapped = Map(Map(item.Data, item.Index), newUserId, item.Data.GatewayId);
                switch (item.State)
                {
                    case EnumDcListElementState.New:
                        var addEntity = db.TblUserSensors.Add(mapped);
                        indexedEntities.Add(item.Index, addEntity);
                        newSensors.Add(new Tuple<EntityEntry<TableUserSensor>, ExSensor>(addEntity, item.Data));
                        break;
                    case EnumDcListElementState.Modified:
                        var updateEntity = db.TblUserSensors.Update(mapped);
                        indexedEntities.Add(item.Index, updateEntity);
                        _sensorCacher.DeleteCachedItem(newUserId, item.Index);
                        newSensors.Add(new Tuple<EntityEntry<TableUserSensor>, ExSensor>(updateEntity, item.Data));
                        break;
                    case EnumDcListElementState.Deleted:
                        db.TblUserSensors.Remove(mapped);
                        _sensorCacher.DeleteCachedItem(newUserId, item.Index);
                        break;
                    case EnumDcListElementState.None:
                        continue;
                }

                result.ElementsStored++;
            }

            try
            {
                await db.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Logging.Log.LogError(e.Message);
                return new DcListStoreResult {StoreResult = new DcStoreResult {ErrorType = EnumServerError.ServerException, ServerExceptionText = e.Message}};
            }

            foreach (var oldIndexEntityPair in indexedEntities)
            {
                result.NewIndex.Add(new DcListStoreResultIndex {BeforeStoreIndex = oldIndexEntityPair.Key, NewIndex = oldIndexEntityPair.Value.Entity.Id});
            }

            foreach (var added in newSensors)
            {
                _sensorCacher.CacheItem(newUserId, added.Item1.Entity.Id, added.Item2);
            }

            return result;
        }

        #endregion
    }
}