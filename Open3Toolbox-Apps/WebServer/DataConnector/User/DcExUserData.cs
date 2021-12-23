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
using Biss.Apps.Base;
using Biss.Dc.Core;
using Database.Context;
using Database.Tables;
using Exchange.Model;
using Microsoft.EntityFrameworkCore;
using WebExchange;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Datenaustausch für DcExCheckUser</para>
    ///     Klasse DcExCheckUser. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public partial class ServerRemoteCalls
    {
        /// <summary>
        ///     Device fordert Daten für DcExUserData
        /// </summary>
        /// <param name="db">Db</param>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        private async Task<ExUserData> GetDcExUserDataFromDb(DatabaseContext db, long deviceId, long userId)
        {
            var dbUser = await db.TblUsers.FirstAsync(u => u.Id == userId);
            return new ExUserData
                   {
                       Id = userId,
                       AgbVersion = dbUser.AgbVersion,
                       DefaultUserLanguage = dbUser.DefaultLanguage,
                       FirstName = dbUser.FirstName,
                       LastName = dbUser.LastName,
                       UserEmail = dbUser.LoginName,
                       LoginName = dbUser.LoginName,
                       UserImageLink = dbUser.UserImage?.DownloadLink ?? string.Empty,
                       PasswordHash = String.Empty
                   };
        }

        #region Interface Implementations

        /// <summary>
        ///     Device fordert Daten für DcExUserData
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        public async Task<ExUserData> GetDcExUserData(long deviceId, long userId)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);
            return await GetDcExUserDataFromDb(db, deviceId, userId);
        }


        /// <summary>
        ///     Device will Daten für DcExNewUserData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        public async Task<DcStoreResult> SetDcExUserData(long deviceId, long userId, ExUserData data)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);
            var newUser = false;
            TableUser user;
            if (userId < 0)
            {
                newUser = true;
                user = new TableUser
                       {
                           CreatedAtUtc = DateTime.UtcNow,
                           PasswordHash = data.PasswordHash,
                           RestPassword = AppCrypt.GeneratePassword()
                       };
            }
            else
            {
                user = await db.TblUsers.FirstOrDefaultAsync(u => u.Id == userId).ConfigureAwait(true);
            }

            user.FirstName = data.FirstName;
            user.LastName = data.LastName;
            user.AgbVersion = data.AgbVersion;
            user.DefaultLanguage = data.DefaultUserLanguage;
            user.LoginName = data.LoginName;

            if (!string.IsNullOrWhiteSpace(data.PasswordHash))
            {
                user.PasswordHash = data.PasswordHash;
            }

            if (newUser)
            {
                await db.TblUsers.AddAsync(user).ConfigureAwait(true);
            }

            await db.SaveChangesAsync().ConfigureAwait(true);

            if (newUser)
            {
                data.Id = user.Id;

                user.FirstAidActive = user.Id % 2 == 0;
                await db.SaveChangesAsync().ConfigureAwait(true);
            }

            data.PasswordHash = string.Empty;

            var r2 = await SendDcExUserData(data, deviceId).ConfigureAwait(true);

            return new DcStoreResult();
        }

        #endregion
    }
}