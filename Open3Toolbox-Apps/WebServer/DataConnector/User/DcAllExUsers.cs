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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biss.Collections;
using Biss.Dc.Core;
using Database.Context;
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
        /// <summary>
        ///     Device fordert Listen Daten für DcExUsers
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
        /// <returns>Daten oder eine Exception auslösen</returns>
        public async Task<List<DcServerListItem<ExUser>>> GetDcExUsers(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);


            //get all users
            if (!db.TblUsers.Any())
            {
                return new List<DcServerListItem<ExUser>>();
            }

            var realStartIndex = Math.Max(startIndex, 0);

            var res = db.TblUsers
                .Where(x => x.Id >= realStartIndex);

            if (elementsToRead > -1)
            {
                res = res.Take((int) elementsToRead);
            }

            var result = res
                .Include(x => x.TblDevices)
                .AsEnumerable()
                .Select(x =>
                {
                    ExUser user = x;
                    user.Devices = new ObservableCollectionFilterable<ExDevice>(x.TblDevices.OrderByDescending(y => y.LastDateTimeUtcOnline));
                    return new DcServerListItem<ExUser> {Data = user, Index = x.Id};
                })
                .ToList();

            return result;
        }

        /// <summary>
        ///     Device will Listen Daten für DcExUsers sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">
        ///     Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb.
        ///     für Chats
        /// </param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        public async Task<DcListStoreResult> StoreDcExUsers(long deviceId, long userId, List<DcStoreListItem<ExUser>> data, long secondId)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            if (!await IsAdmin(db, userId).ConfigureAwait(false))
            {
                throw new Exception("Only for Admin Users");
            }

            foreach (var dcStoreListItem in data)
            {
                var user = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == dcStoreListItem.Index).ConfigureAwait(false);

                UpdateUserSettings(user, dcStoreListItem.Data);

                await db.SaveChangesAsync().ConfigureAwait(false);
            }

            return new DcListStoreResult();
        }

        private async Task<bool> IsAdmin(DatabaseContext db, long userId)
        {
            var user = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == userId)
                .ConfigureAwait(false);

            return user.IsAdmin;
        }
    }
}