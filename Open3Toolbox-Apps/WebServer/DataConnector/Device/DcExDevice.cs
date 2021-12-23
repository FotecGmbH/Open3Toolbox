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
        ///     Device fordert Daten für DcExDevice
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        public Task<ExDevice> GetDcExDevice(long deviceId, long userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Device will Daten für DcExNewUserData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        public async Task<DcStoreResult> SetDcExDevice(long deviceId, long userId, ExDevice data)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);
            var device = await db.TblDevices
                .Include(x => x.TblUser)
                .FirstOrDefaultAsync(d => d.Id == deviceId)
                .ConfigureAwait(true);

            #region DeviceInfo

            device.DeviceHardwareId = data.DeviceHardwareId;
            device.Plattform = data.Plattform;
            device.DeviceIdiom = data.DeviceIdiom;
            device.OperatingSystemVersion = data.OperatingSystemVersion;
            device.DeviceType = data.DeviceType;
            device.DeviceName = data.DeviceName;
            device.Model = data.Model;
            device.Manufacturer = data.Manufacturer;

            #endregion

            #region DeviceBase

            device.DeviceToken = data.DeviceToken;
            device.AppVersion = data.AppVersion;
            // IsAppRunning
            // LastOnline
            device.PushTags = data.PushTags;
            device.ScreenResolution = data.ScreenResolution;

            device.TimeDifference = data.TimeDifference;

            #endregion

            if (device.TblUser != null)
            {
                device.TblUser.TimeDifference = data.TimeDifference;
            }

            await db.SaveChangesAsync().ConfigureAwait(true);
            return new DcStoreResult();
        }
    }
}