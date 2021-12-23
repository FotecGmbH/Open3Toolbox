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
using Database.Tables.Statistics;
using Exchange.Model.Statistics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WebExchange;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Datenaustausch für die logische Ansichten des Benutzers.</para>
    ///     Klasse ServerRemoteCalls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    /// <seealso cref="WebServer.DataConnector.ServerRemoteCallBase" />
    /// <seealso cref="WebServer.DataConnector.IServerRemoteCalls" />
    public partial class ServerRemoteCalls
    {
        /// <summary>
        ///     Löscht die Unteransichten (auch final), rekursive aus dem Datenbank, wird benötigt.
        /// </summary>
        /// <param name="subViews">Die Unteransichten.</param>
        /// <param name="finalSubViews">Die final Unteransichten.</param>
        /// <param name="subView">Der zulöschende Unteransicht.</param>
        private static void SubViewDeleteHelper(DbSet<TableSubView> subViews, DbSet<TableFinalSubView> finalSubViews, TableSubView subView)
        {
            foreach (var sV in subViews.Where(sV => sV.SubViewId == subView.Id))
            {
                SubViewDeleteHelper(subViews, finalSubViews, sV);
            }

            foreach (var fSV in finalSubViews.Where(fSV => fSV.SubViewId == subView.Id))
            {
                finalSubViews.Remove(fSV);
            }

            subViews.Remove(subView);
        }

        /// <summary>
        ///     Holt der vorbereitete LINQ Query für diesen <see cref="Type" />.
        /// </summary>
        /// <param name="subViews">Die Unteransichten.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Der LINQ Query.</returns>
        private IQueryable<TableSubView> GetSubViewsQuery(DbSet<TableSubView> subViews, long userId)
        {
            return subViews
                ;
        }

        /// <summary>
        ///     Holt der vorbereitete LINQ Query für diesen <see cref="Type" />.
        /// </summary>
        /// <param name="finalSubViews">Die final Unteransichten.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Der LINQ Query.</returns>
        private IQueryable<TableFinalSubView> GetFinalSubViewsQuery(DbSet<TableFinalSubView> finalSubViews, long userId)
        {
            return finalSubViews
                ;
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="subView">Der Unteransicht.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private ExSubView Map(TableSubView subView)
        {
            var toReturn = new ExSubView
                           {
                               IsPartOfMainView = subView.IsPartOfMainView,
                               Name = subView.Name,
                               SubViewId = subView.SubViewId,
                           };

            return toReturn;
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="finalSubView">Der final Unteransicht.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private ExFinalSubView Map(TableFinalSubView finalSubView)
        {
            var toReturn = new ExFinalSubView
                           {
                               IsPartOfMainView = finalSubView.IsPartOfMainView,
                               Name = finalSubView.Name,
                               SubViewId = finalSubView.SubViewId,
                           };

            return toReturn;
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="subView">Der Unteransicht.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="subViewId">Der Unteransicht Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private TableSubView Map(ExSubView subView, long userId, long subViewId)
        {
            var toReturn = new TableSubView
                           {
                               IsPartOfMainView = subView.IsPartOfMainView,
                               Id = subViewId < 0 ? 0 : subViewId,
                               Name = subView.Name,
                               SubViewId = subView.SubViewId,
                           };

            return toReturn;
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="finalSubView">Der final Unteransicht.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="finalSubViewId">Der final Unteransicht Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private TableFinalSubView Map(ExFinalSubView finalSubView, long userId, long finalSubViewId)
        {
            var toReturn = new TableFinalSubView
                           {
                               IsPartOfMainView = finalSubView.IsPartOfMainView,
                               Id = finalSubViewId < 0 ? 0 : finalSubViewId,
                               Name = finalSubView.Name,
                               SubViewId = finalSubView.SubViewId,
                           };

            return toReturn;
        }

        #region Interface Implementations

        /// <summary>
        ///     Device fordert Listen Daten für DcExSubViews
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
        public async Task<List<DcServerListItem<ExSubView>>> GetDcExSubViews(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            /// <param name="startIndex"/> == 0 => Gebe nur <see cref="ExSubView"/>s wo <see cref="ExSubView.IsPartOfMainView"/> ist <see cref="true"/>
            /// <param name="startIndex"/> > 0 => Gebe nur <see cref="ExSubView"/>s wo <see cref="TableSubView.Id"/> == <param name="startIndex"/> und <see cref="TableSubView.Id"/> == <see cref="TableSubView.SubViewId"/>
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var subViewTablesQuery = GetSubViewsQuery(db.TblSubViews, newUserId);
            if (startIndex == 0)
            {
                subViewTablesQuery = subViewTablesQuery.Where(sV => sV.IsPartOfMainView);
            }

            if (startIndex > 0)
            {
                subViewTablesQuery = subViewTablesQuery.Where(sV => sV.Id == startIndex || sV.SubViewId == startIndex);
            }

            var sortIndex = 0;
            var result = new List<DcServerListItem<ExSubView>>();
            foreach (var subViewTable in subViewTablesQuery)
            {
                var subView = Map(subViewTable);
                result.Add(new DcServerListItem<ExSubView> {Data = subView, Index = subViewTable.Id, SortIndex = ++sortIndex});
            }

            return result;
        }

        /// <summary>
        ///     Device will Listen Daten für DcExSubViews sichern
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
        public async Task<DcListStoreResult> StoreDcExSubViews(long deviceId, long userId, List<DcStoreListItem<ExSubView>> data, long secondId)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var result = new DcListStoreResult {ElementsStored = 0, NewIndex = new List<DcListStoreResultIndex>(), StoreResult = new DcStoreResult {ErrorType = EnumServerError.None, ServerExceptionText = ""}};
            var indexedEntities = new Dictionary<long, EntityEntry<TableSubView>>(); // Alte index zu entity
            foreach (var item in data)
            {
                var mapped = Map(item.Data, newUserId, item.Index);
                switch (item.State)
                {
                    case EnumDcListElementState.New:
                        indexedEntities.Add(item.Index, db.TblSubViews.Add(mapped));
                        break;
                    case EnumDcListElementState.Modified:
                        indexedEntities.Add(item.Index, db.TblSubViews.Update(mapped));
                        break;
                    case EnumDcListElementState.Deleted:
                        SubViewDeleteHelper(db.TblSubViews, db.TblFinalSubViews, mapped);
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
                result.StoreResult.ErrorType = EnumServerError.ServerException;
                result.StoreResult.ServerExceptionText = e.Message;
            }

            foreach (var oldIndexEntityPair in indexedEntities)
            {
                result.NewIndex.Add(new DcListStoreResultIndex {BeforeStoreIndex = oldIndexEntityPair.Key, NewIndex = oldIndexEntityPair.Value.Entity.Id});
            }

            return result;
        }

        /// <summary>
        ///     Device fordert Listen Daten für DcExFinalSubViews
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
        public async Task<List<DcServerListItem<ExFinalSubView>>> GetDcExFinalSubViews(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            /// <param name="startIndex"/> == 0 && filter == "" => Gebe nur <see cref="ExFinalSubView"/>s wo <see cref="ExFinalSubView.IsPartOfMainView"/> ist <see cref="true"/>
            /// <param name="startIndex"/> > 0 => Gebe nur <see cref="ExFinalSubView"/>s wo <param name="startIndex"/> == <see cref="TableFinalSubView.Id"/>
            /// <param name="filter"/> > 0 => Gebe nur <see cref="ExFinalSubView"/>s wo <param name="filter"/> == <see cref="TableFinalSubView.SubViewId"/>
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var finalSubViewTablesQuery = GetFinalSubViewsQuery(db.TblFinalSubViews, newUserId);
            if (startIndex == 0 && string.IsNullOrWhiteSpace(filter))
            {
                finalSubViewTablesQuery = finalSubViewTablesQuery.Where(fsV => fsV.IsPartOfMainView);
            }

            if (startIndex > 0)
            {
                finalSubViewTablesQuery = finalSubViewTablesQuery.Where(fsV => fsV.Id == startIndex);
            }

            if (long.TryParse(filter, out var subViewId) && subViewId > 0)
            {
                finalSubViewTablesQuery = finalSubViewTablesQuery.Where(fsV => fsV.SubViewId == subViewId);
            }

            var sortIndex = 0;
            var result = new List<DcServerListItem<ExFinalSubView>>();
            foreach (var finalSubViewTable in finalSubViewTablesQuery)
            {
                var finalSubView = Map(finalSubViewTable);
                result.Add(new DcServerListItem<ExFinalSubView> {Data = finalSubView, Index = finalSubViewTable.Id, SortIndex = ++sortIndex});
            }

            return result;
        }

        /// <summary>
        ///     Device will Listen Daten für DcExFinalSubViews sichern
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
        public async Task<DcListStoreResult> StoreDcExFinalSubViews(long deviceId, long userId, List<DcStoreListItem<ExFinalSubView>> data, long secondId)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var result = new DcListStoreResult {ElementsStored = 0, NewIndex = new List<DcListStoreResultIndex>(), StoreResult = new DcStoreResult {ErrorType = EnumServerError.None, ServerExceptionText = ""}};
            var indexedEntities = new Dictionary<long, EntityEntry<TableFinalSubView>>(); // Old index to entity
            foreach (var item in data)
            {
                var mapped = Map(item.Data, newUserId, item.Index);
                switch (item.State)
                {
                    case EnumDcListElementState.New:
                        indexedEntities.Add(item.Index, db.TblFinalSubViews.Add(mapped));
                        break;
                    case EnumDcListElementState.Modified:
                        indexedEntities.Add(item.Index, db.TblFinalSubViews.Update(mapped));
                        break;
                    case EnumDcListElementState.Deleted:
                        db.TblFinalSubViews.Remove(mapped);
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

            return result;
        }

        #endregion
    }
}