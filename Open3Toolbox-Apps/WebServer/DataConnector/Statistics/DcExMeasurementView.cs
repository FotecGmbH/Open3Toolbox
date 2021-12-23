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
using ExchangeLibrary.DataBaseData.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WebExchange;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Datenaustausch für die Ansicht und Logischen Messungen.</para>
    ///     Klasse ServerRemoteCalls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    /// <seealso cref="WebServer.DataConnector.ServerRemoteCallBase" />
    /// <seealso cref="WebServer.DataConnector.IServerRemoteCalls" />
    public partial class ServerRemoteCalls
    {
        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="measurementView">Der Messungsansicht.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private ExMeasurementView Map(TableMeasurementView measurementView)
        {
            return new ExMeasurementView
                   {
                       FinalSubViewId = measurementView.FinalSubViewId,
                       Name = measurementView.Name,
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="measurementView">Der Messungsansicht.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="measurementId">Der Messungs Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private TableMeasurementView Map(ExMeasurementView measurementView, long userId, long measurementId)
        {
            return new TableMeasurementView
                   {
                       // TblUserId = userId, // TODO: Comment back
                       FinalSubViewId = measurementView.FinalSubViewId,
                       MeasurementId = measurementId < 0 ? 0 : measurementId, // Id == 0 => Neuer Instanz.
                       Name = measurementView.Name,
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="measurement">The measurement.</param>
        /// <param name="lastMeasurementData">The last measurement data.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static ExMeasurementDetails Map(TablePublishedMeasurement measurement, TableMeasurementData lastMeasurementData)
        {
            return new ExMeasurementDetails
                   {
                       Name = measurement.Name,
                       Description = measurement.Description,
                       Port = measurement.Port,
                       LastMeasured = lastMeasurementData is null ? null : lastMeasurementData.TimeStamp,
                       LastMeasuredValue = lastMeasurementData is null ? 0 : lastMeasurementData.Value,
                   };
        }

        /// <summary>
        ///     Mapt die angegebenen <see cref="Type" />s.
        /// </summary>
        /// <param name="measurementData">Der Messungsdatei.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static ExMeasurementData Map(TableMeasurementData measurementData)
        {
            return new ExMeasurementData
                   {
                       MeasurementId = measurementData.MeasurementId,
                       Value = measurementData.Value,
                       TimeStamp = measurementData.TimeStamp,
                       Altitude = measurementData.Altitude,
                       Latitude = measurementData.Latitude,
                       Longitude = measurementData.Longitude,
                   };
        }

        #region Interface Implementations

        /// <summary>
        ///     Device fordert Listen Daten für DcExMeasurementViews
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
        public Task<List<DcServerListItem<ExMeasurementView>>> GetDcExMeasurementViews(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            /// <param name="startIndex"/> == 0 => Gebe alles zurück
            /// <param name="startIndex"/> > 0 => Gebe nur <see cref="ExMeasurementView"/>s wo <param name="startIndex"/> == <see cref="TableMeasurementView.MeasurementId"/>
            /// <param name="filter"/> > 0 => Gebe nur <see cref="ExMeasurementView"/>s wo <param name="filter"/> == <see cref="TableMeasurementView.FinalSubViewId"/>
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var measurementViewTablesQuery = db.TblMeasurementViews.Where(mV => mV.MeasurementId > 0);
            if (startIndex > 0)
            {
                measurementViewTablesQuery = measurementViewTablesQuery.Where(mV => mV.MeasurementId == startIndex);
            }
            else if (filter == $"{nameof(ExMeasurementView.FinalSubViewId)}==null")
            {
                measurementViewTablesQuery = measurementViewTablesQuery.Where(mV => mV.FinalSubViewId == null);
            }
            else if (long.TryParse(filter, out var measurementId) && measurementId > 0)
            {
                measurementViewTablesQuery = measurementViewTablesQuery.Where(mV => mV.FinalSubViewId == measurementId);
            }

            var sortIndex = 0;
            var result = new List<DcServerListItem<ExMeasurementView>>();
            foreach (var measurementViewTable in measurementViewTablesQuery)
            {
                var subView = Map(measurementViewTable);
                result.Add(new DcServerListItem<ExMeasurementView> {Data = subView, Index = measurementViewTable.MeasurementId, SortIndex = ++sortIndex});
            }

            return Task.FromResult(result);
        }

        /// <summary>
        ///     Device will Listen Daten für DcExMeasurementViews sichern
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
        public async Task<DcListStoreResult> StoreDcExMeasurementViews(long deviceId, long userId, List<DcStoreListItem<ExMeasurementView>> data, long secondId)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var result = new DcListStoreResult {ElementsStored = 0, NewIndex = new List<DcListStoreResultIndex>(), StoreResult = new DcStoreResult {ErrorType = EnumServerError.None, ServerExceptionText = ""}};
            var indexedEntities = new Dictionary<long, EntityEntry<TableMeasurementView>>(); // Alte index to entity
            foreach (var item in data)
            {
                var mapped = Map(item.Data, newUserId, item.Index);
                switch (item.State)
                {
                    case EnumDcListElementState.New:
                        indexedEntities.Add(item.Index, db.TblMeasurementViews.Add(mapped));
                        break;
                    case EnumDcListElementState.Modified:
                        indexedEntities.Add(item.Index, db.TblMeasurementViews.Update(mapped));
                        break;
                    case EnumDcListElementState.Deleted:
                        // Aktoren sind physisch vorhanden, während der View System nur für die bequemen Verwaltung sorgt, daher Remove => vom FinalSubView Entfernen.
                        mapped.FinalSubView = null;
                        mapped.FinalSubViewId = null;
                        indexedEntities.Add(item.Index, db.TblMeasurementViews.Update(mapped));
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
                result.NewIndex.Add(new DcListStoreResultIndex {BeforeStoreIndex = oldIndexEntityPair.Key, NewIndex = oldIndexEntityPair.Value.Entity.MeasurementId});
            }

            return result;
        }

        /// <summary>
        ///     Device fordert Listen Daten für DcExMeasurementDetails
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
        public Task<List<DcServerListItem<ExMeasurementDetails>>> GetDcExMeasurementDetails(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var measurementTablesQuery = db.TblPublishedMeasurements.Where(mV => mV.Id == startIndex);

            var sortIndex = 0;
            var result = new List<DcServerListItem<ExMeasurementDetails>>();
            foreach (var measurementTable in measurementTablesQuery)
            {
                var measurement = Map(measurementTable, db.TblMeasurementData.Where(mD => mD.MeasurementId == measurementTable.Id).OrderByDescending(mD => mD.TimeStamp).FirstOrDefault());
                result.Add(new DcServerListItem<ExMeasurementDetails> {Data = measurement, Index = measurementTable.Id, SortIndex = ++sortIndex});
            }

            return Task.FromResult(result);
        }

        /// <summary>
        ///     Device will Listen Daten für DcExMeasurementDetails sichern
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
        public Task<DcListStoreResult> StoreDcExMeasurementDetails(long deviceId, long userId, List<DcStoreListItem<ExMeasurementDetails>> data, long secondId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Device fordert Listen Daten für DcExMeasurementData
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
        public Task<List<DcServerListItem<ExMeasurementData>>> GetDcExMeasurementData(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);

            var measurementDataTablesQuery = db.TblMeasurementData.Where(mV => mV.MeasurementId == startIndex);

            var sortIndex = 0;
            var result = new List<DcServerListItem<ExMeasurementData>>();
            foreach (var measurementDataTable in measurementDataTablesQuery)
            {
                var measurementData = Map(measurementDataTable);
                result.Add(new DcServerListItem<ExMeasurementData> {Data = measurementData, Index = measurementDataTable.Id, SortIndex = ++sortIndex});
            }

            return Task.FromResult(result);
        }

        /// <summary>
        ///     Device will Listen Daten für DcExMeasurementData sichern
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
        /// <exception cref="System.NotImplementedException">Not Needed.</exception>
        public Task<DcListStoreResult> StoreDcExMeasurementData(long deviceId, long userId, List<DcStoreListItem<ExMeasurementData>> data, long secondId)
        {
            throw new NotImplementedException("Not Needed.");
        }

        #endregion
    }
}