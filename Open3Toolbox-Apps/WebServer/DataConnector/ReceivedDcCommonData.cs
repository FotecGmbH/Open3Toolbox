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
using System.Linq;
using System.Threading.Tasks;
using Biss.Dc.Core;
using Biss.Log.Producer;
using Biss.Serialize;
using Database.Context;
using Database.Tables;
using Exchange.DataConnector;
using Exchange.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebExchange;
using WebServer.Helper;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Datenaustausch</para>
    ///     Klasse DcExCheckUser. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public partial class ServerRemoteCalls
    {
        /// <summary>
        ///     Willkommen E-Mail senden
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id vom Benutzer</param>
        /// <param name="data">Daten</param>
        /// <returns>Einen Task</returns>
        private async Task SendWellcomeEmail(long deviceId, long userId, string data)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);
            var id = userId;
            if (id < 0)
            {
                long.TryParse(data, out id);
            }

            var user = await db.TblUsers.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new Exception($"SendWellcomeEmail: user with id {userId} not found!");
            }

            MailHelper mHelper = new MailHelper();
            await mHelper.SendValidationMail(user, deviceId);
        }

        /// <summary>
        ///     Telemetriedaten sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id vom Benutzer</param>
        /// <param name="data">Daten</param>
        /// <returns>Einen Task</returns>
        private async Task StoreTelemetry(long deviceId, long userId, string data)
        {
            var info = BissDeserialize.FromJson<ExViewState>(data);

            await using var db = new DatabaseContext(WebConstants.ConnectionString);
            TableViewState dbItem;

            if (info.IsAppearing)
            {
                // Add to DB
                dbItem = new TableViewState
                         {
                             TblDeviceId = deviceId,
                             TblUserId = userId > 0 ? userId : null,

                             From = info.CurrentDateTime,
                             To = DateTime.MaxValue,
                             ViewName = info.ViewName,
                         };

                db.TblViewStates.Add(dbItem);
            }
            else
            {
                // set DB Item Closed
                dbItem = db.TblViewStates.Where(x =>
                        x.TblDeviceId == deviceId &&
                        (x.TblUserId == null || x.TblUserId.Value == userId) &&
                        x.ViewName == info.ViewName &&
                        x.To == DateTime.MaxValue)
                    .OrderByDescending(x => x.From)
                    .FirstOrDefault()!;

                if (dbItem == null!)
                {
                    // Add to DB
                    dbItem = new TableViewState
                             {
                                 TblDeviceId = deviceId,
                                 TblUserId = userId > 0 ? userId : null,

                                 From = DateTime.MinValue,
                                 To = info.CurrentDateTime,
                                 ViewName = info.ViewName,
                             };

                    db.TblViewStates.Add(dbItem);
                }
                else
                {
                    dbItem.To = info.CurrentDateTime;
                    if (dbItem.TblUserId == null && userId > 0)
                    {
                        dbItem.TblUserId = userId;
                    }
                }
            }

            await db.SaveChangesAsync().ConfigureAwait(true);
        }

        /// <summary>
        ///     Account löschen
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id vom Benutzer</param>
        /// <param name="data">Daten</param>
        /// <returns>Einen Task</returns>
        private async Task DeleteAccount(long deviceId, long userId, string data)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            var user = db.TblUsers.FirstOrDefault(x => x.Id == userId);

            if (user == null)
            {
                return;
            }

            // TODO ich setz mal nur den Login auf Deleted, man kann also sich nicht mehr anmelden, "anonymisierte" Daten bleiben aber erhalten
            user.LoginName = "DELETED";
            user.Locked = true;

            await db.SaveChangesAsync().ConfigureAwait(true);

            foreach (var client in ClientConnection.GetClients())
            {
                await SendCommonData(client.DeviceId, EnumDcCommonCommandsClient.LogoutUser, string.Empty).ConfigureAwait(true);
            }
        }

        #region Interface Implementations

        /// <summary>
        ///     Allgemeine Daten vom Device empfangen
        /// </summary>
        /// <param name="deviceId">Geräte Id</param>
        /// <param name="userId">Benutzer Id</param>
        /// <param name="data">Daten</param>
        /// <returns>Einen Task</returns>
        public async Task ReceivedDcCommonData(long deviceId, long userId, DcCommonData data)
        {
            var r = Enum.TryParse<EnumDcCommonCommand>(data.Key, true, out var t);
            if (!r)
            {
                Logging.Log.LogWarning($"Dc Get Common Data: {data.Key}:{data.Value} from device {deviceId}");
                return;
            }

            switch (t)
            {
                case EnumDcCommonCommand.SendWellcomeEmail:
                    await SendWellcomeEmail(deviceId, userId, data.Value);
                    return;
                case EnumDcCommonCommand.TelemetryData:
                    await StoreTelemetry(deviceId, userId, data.Value);
                    return;
                case EnumDcCommonCommand.DeleteAccount:
                    await DeleteAccount(deviceId, userId, data.Value);
                    return;
                default:
                    throw new ArgumentOutOfRangeException(nameof(t));
            }
        }

        #endregion
    }
}