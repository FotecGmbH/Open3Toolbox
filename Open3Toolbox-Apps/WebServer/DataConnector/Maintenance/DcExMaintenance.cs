// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       WebServer
// 
// Released under MIT

using System.Threading.Tasks;
using Biss.Dc.Core;
using Database.Context;
using Database.Tables;
using Exchange.Model;
using Microsoft.EntityFrameworkCore;
using WebExchange;

// ReSharper disable once CheckNamespace
namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Datenaustausch für DC</para>
    ///     Klasse ServerRemoteCalls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public partial class ServerRemoteCalls
    {
        #region Interface Implementations

        /// <summary>
        ///     Device fordert Daten für DcExMaintenance
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        public async Task<ExMaintenance> GetDcExMaintenance(long deviceId, long userId)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            // sollte nur ein Eintrag in Tabelle sein
            var settings = await db.TblSettings.FirstOrDefaultAsync().ConfigureAwait(false);

            if (settings is null || !settings.MaintenanceActive)
            {
                return new ExMaintenance();
            }

            return new ExMaintenance
                   {
                       MaintenanceActive = settings.MaintenanceActive,
                       MaintenanceText = settings.MaintenanceText
                   };
        }

        /// <summary>
        ///     Device will Daten für DcExNewUserData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        public async Task<DcStoreResult> SetDcExMaintenance(long deviceId, long userId, ExMaintenance data)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            // sollte nur ein Eintrag in Tabelle sein
            var settings = await db.TblSettings.FirstOrDefaultAsync().ConfigureAwait(false);

            // neues Setting anlegen wenn keins vorhanden (erster Zugriff -> Tabelle evtl. leer?)
            if (settings is null)
            {
                var newSettings = new TableSetting
                                  {
                                      MaintenanceActive = data.MaintenanceActive,
                                      MaintenanceText = data.MaintenanceText
                                  };
                await db.TblSettings.AddAsync(newSettings).ConfigureAwait(false);
            }
            else
            {
                settings.MaintenanceActive = data.MaintenanceActive;
                settings.MaintenanceText = data.MaintenanceText;
            }

            await db.SaveChangesAsync().ConfigureAwait(false);
            return new DcStoreResult();
        }

        #endregion
    }
}