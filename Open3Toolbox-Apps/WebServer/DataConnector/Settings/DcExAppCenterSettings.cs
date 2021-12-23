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

using System;
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
    ///     <para>Datenaustausch für DcAppCenterSettings</para>
    ///     Klasse ServerRemoteCalls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public partial class ServerRemoteCalls
    {
        #region Interface Implementations

        /// <summary>
        ///     Device fordert Daten für DcAppCenterSettings
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        public async Task<ExAppCenterSettings> GetDcAppCenterSettings(long deviceId, long userId)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            var settings = await db.TblSettings.FirstOrDefaultAsync().ConfigureAwait(false);

            if (settings is null)
            {
                return new ExAppCenterSettings();
            }

            return new ExAppCenterSettings
                   {
                       AppcenterLinkIos = settings.AppcenterLinkIos,
                       AppcenterLinkDroid = settings.AppcenterLinkDroid
                   };
        }

        /// <summary>
        ///     Device will Daten für DcExNewUserData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        public async Task<DcStoreResult> SetDcAppCenterSettings(long deviceId, long userId, ExAppCenterSettings data)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            var settings = await db.TblSettings.FirstOrDefaultAsync().ConfigureAwait(false);

            // neues Setting anlegen wenn keins vorhanden (erster Zugriff -> Tabelle evtl. leer?)
            if (settings is null)
            {
                var newSettings = new TableSetting
                                  {
                                      AppcenterLinkDroid = data.AppcenterLinkDroid,
                                      AppcenterLinkIos = data.AppcenterLinkIos
                                  };
                await db.TblSettings.AddAsync(newSettings).ConfigureAwait(false);
            }
            else
            {
                settings.AppcenterLinkIos = data.AppcenterLinkIos;
                settings.AppcenterLinkDroid = data.AppcenterLinkDroid;
            }

            await db.SaveChangesAsync().ConfigureAwait(false);
            return new DcStoreResult();
        }

        #endregion
    }
}