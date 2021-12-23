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
using Biss.Log.Producer;
using Database.Context;
using Database.Tables.ConfigurationTool;
using Database.Tables.Statistics;
using Exchange.Model.Statistics;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WebExchange;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Datenaustausch für die Ansicht und Logischen Aktoren.</para>
    ///     Klasse ServerRemoteCalls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    /// <seealso cref="WebServer.DataConnector.ServerRemoteCallBase" />
    /// <seealso cref="WebServer.DataConnector.IServerRemoteCalls" />
    public partial class ServerRemoteCalls
    {
        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="actorView">Der Aktoransicht.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private ExActorView Map(TableActorView actorView)
        {
            return new ExActorView
                   {
                       FinalSubViewId = actorView.FinalSubViewId,
                       Name = actorView.Name,
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="actorView">Der Aktoransicht.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="actorId">Der Aktor Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private TableActorView Map(ExActorView actorView, long userId, long actorId)
        {
            return new TableActorView
                   {
                       FinalSubViewId = actorView.FinalSubViewId,
                       ActorId = actorId < 0 ? 0 : actorId, // Id == 0 => Neuer Instanz.
                       Name = actorView.Name,
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="actor">Der Aktor.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static ExActorDetails Map(TablePublishedActor actor)
        {
            return new ExActorDetails
                   {
                       Name = actor.Name,
                       Description = actor.Description,
                       Port = actor.Port,
                       SetterType = actor.SetterType,
                       Value = actor.Value,
                   };
        }

        #region Interface Implementations

        /// <summary>
        ///     Device fordert Listen Daten für DcExActorViews
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
        public async Task<List<DcServerListItem<ExActorView>>> GetDcExActorViews(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            /// <param name="startIndex"/> == -1 => Gebe alles zurück
            /// <param name="startIndex"/> > 0 => Gebe nur <see cref="ExActorView"/>s wo <param name="startIndex"/> == <see cref="TableActorView.ActorId"/>
            /// <param name="filter"/> > 0 => Gebe nur <see cref="ExActorView"/>s wo <param name="filter"/> == <see cref="TableActorView.FinalSubViewId"/>
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var actorViewTablesQuery = db.TblActorViews.Where(aV => aV.ActorId > 0);
            if (startIndex > 0)
            {
                actorViewTablesQuery = actorViewTablesQuery.Where(aV => aV.ActorId == startIndex);
            }
            else if (filter == $"{nameof(ExActorView.FinalSubViewId)}==null")
            {
                actorViewTablesQuery = actorViewTablesQuery.Where(aV => aV.FinalSubViewId == null);
            }
            else if (long.TryParse(filter, out var actorId) && actorId > 0)
            {
                actorViewTablesQuery = actorViewTablesQuery.Where(aV => aV.FinalSubViewId == actorId);
            }

            var sortIndex = 0;
            var result = new List<DcServerListItem<ExActorView>>();
            foreach (var actorViewTable in actorViewTablesQuery)
            {
                var subView = Map(actorViewTable);
                result.Add(new DcServerListItem<ExActorView> {Data = subView, Index = actorViewTable.ActorId, SortIndex = ++sortIndex});
            }

            return result;
        }

        /// <summary>
        ///     Device will Listen Daten für DcExActorViews sichern
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
        public async Task<DcListStoreResult> StoreDcExActorViews(long deviceId, long userId, List<DcStoreListItem<ExActorView>> data, long secondId)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var result = new DcListStoreResult {ElementsStored = 0, NewIndex = new List<DcListStoreResultIndex>(), StoreResult = new DcStoreResult {ErrorType = EnumServerError.None, ServerExceptionText = ""}};
            var indexedEntities = new Dictionary<long, EntityEntry<TableActorView>>(); // Alte index to entity
            foreach (var item in data)
            {
                var mapped = Map(item.Data, newUserId, item.Index);
                switch (item.State)
                {
                    case EnumDcListElementState.New:
                        indexedEntities.Add(item.Index, db.TblActorViews.Add(mapped));
                        break;
                    case EnumDcListElementState.Modified:
                        indexedEntities.Add(item.Index, db.TblActorViews.Update(mapped));
                        break;
                    case EnumDcListElementState.Deleted:
                        mapped.FinalSubView = null;
                        mapped.FinalSubViewId = null;
                        indexedEntities.Add(item.Index, db.TblActorViews.Update(mapped));
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
                result.NewIndex.Add(new DcListStoreResultIndex {BeforeStoreIndex = oldIndexEntityPair.Key, NewIndex = oldIndexEntityPair.Value.Entity.ActorId});
            }

            return result;
        }

        /// <summary>
        ///     Device fordert Listen Daten für DcExActorDetails
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
        public Task<List<DcServerListItem<ExActorDetails>>> GetDcExActorDetails(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var actorTablesQuery = db.TblPublishedActors.Where(aV => aV.Id == startIndex);

            var sortIndex = 0;
            var result = new List<DcServerListItem<ExActorDetails>>();
            foreach (var actorViewTable in actorTablesQuery)
            {
                var actor = Map(actorViewTable);
                result.Add(new DcServerListItem<ExActorDetails> {Data = actor, Index = actorViewTable.Id, SortIndex = ++sortIndex});
            }

            return Task.FromResult(result);
        }

        /// <summary>
        ///     Device will Listen Daten für DcExActorDetails sichern
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
        public Task<DcListStoreResult> StoreDcExActorDetails(long deviceId, long userId, List<DcStoreListItem<ExActorDetails>> data, long secondId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}