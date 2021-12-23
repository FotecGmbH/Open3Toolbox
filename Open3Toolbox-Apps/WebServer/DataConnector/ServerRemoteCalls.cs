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
using Biss.Dc.Core;
using Biss.Log.Producer;
using Database.Context;
using Exchange;
using Exchange.Model;
using Exchange.Model.ConfigurationTool;
using Exchange.Services.ConfigurationTool;
using Exchange.Services.ConfigurationTool.Interfaces;
using ExConfigExchange.Services;
using ExConfigExchange.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebExchange;

namespace WebServer.DataConnector
{
    /// <summary>
    ///     <para>Dc Implementierung der Funktionen am Server</para>
    ///     Klasse ServerRemoteCalls. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public partial class ServerRemoteCalls : ServerRemoteCallBase, IServerRemoteCalls
    {
        // For now...
        private static readonly IExConfigItemManager _configItemManager = new ExConfigItemManager(new LocalObjectExConfigItemTemplateCacher());
        private static readonly IExConfigurableManager _configurableManager = new ExConfigurableManager(_configItemManager, new LocalExConfigurableCacher());
        private static readonly IExCacher<ExProject> _projectCacher = new LocalCacher<ExProject>();
        private static readonly IExCacher<ExGateway> _gatewayCacher = new LocalCacher<ExGateway>();
        private static readonly IExCacher<ExSensor> _sensorCacher = new LocalCacher<ExSensor>();

        #region Interface Implementations

        /// <summary>Neues File wurde von einem Client empfangen</summary>
        /// <param name="deviceId">Device Id</param>
        /// <param name="userId">User Id - kann auch -1 sein - also kein User</param>
        /// <param name="fileName">Originaldateiname</param>
        /// <param name="file">Datei</param>
        /// <param name="commonData">Allgemeine zusätzliche Infos</param>
        /// <returns>Ergebnis des transfers</returns>
        public async Task<DcTransferFileResult> TransferFile(long deviceId, long userId, string fileName, List<byte> file, string commonData)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            var data = JsonConvert.DeserializeObject<ExFileUploadData>(commonData);

            await db.SaveChangesAsync().ConfigureAwait(true);

            return new DcTransferFileResult
                   {
                       CommonData = string.Empty,
                       FileLink = string.Empty,
                       StoreResult = new DcStoreResult()
                   };
        }

        /// <summary>
        ///     Neues Gerät in der Datenbank anlegen.
        /// </summary>
        /// <returns>Db Id des neuen Gerätes</returns>
        public long RegisterNewDevice()
        {
            return -1;
        }

        /// <summary>
        ///     Status (verbunden / nicht verbunden) eines Gerätes hat sich geändert.
        ///     Status in der DB ablegen bei Bedarf
        /// </summary>
        /// <param name="deviceId">id des Gerätes</param>
        /// <param name="state">Status</param>
        public async void DeviceConnectionChanged(long deviceId, EnumDcConnectionState state, Exception? exception = null)
        {
            if (exception != null)
            {
                Logging.Log.LogError($"Error:{exception}");
            }
        }

        /// <summary>
        ///     Benutzer Id anhand des Login (EMail) aus der DB laden. Sollte kein Entrag existieren dann -1 als Rückgabe
        /// </summary>
        /// <param name="loginName">E-Mail, Telefonnummer, was auch immer ...</param>
        /// <param name="deviceId">Device Id aus Datenbank</param>
        /// <returns>UserId</returns>
        public long GetUserIdByLoginName(string loginName, long deviceId)
        {
            Logging.Log.LogInfo($"LoginNameCheckExist {loginName}");
            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var em = loginName?.ToLower() ?? string.Empty;
            if (!db.TblUsers.Any(u => u.LoginName == em))
            {
                return -1;
            }

            var id = db.TblUsers.FirstOrDefault(u => u.LoginName == em)?.Id ?? -1;

            return id;
        }

        /// <summary>
        ///     Einen Benutzer anmelden. Bei Bedarf auch das Device dem Benutzer zuordnen.
        /// </summary>
        /// <param name="infos">Aktuelle Daten</param>
        /// <param name="userPasswortHash">Wenn sich der User einloggt - Passwort (Hash)</param>
        /// <returns>Aktualisierte Daten (bei Bedarf). null bei Fehler!</returns>
        public ExDcCoreInfos LoginUser(ExDcCoreInfos infos, string userPasswortHash)
        {
            if (infos == null)
            {
                throw new ArgumentNullException(nameof(infos));
            }

            using var db = new DatabaseContext(WebConstants.ConnectionString);
            var user = db.TblUsers.FirstOrDefault(u => u.Id == infos.UserId);
            var device = db.TblDevices.FirstOrDefault(d => d.Id == infos.DeviceId);
            var haserror = false;

            if (user == null || device == null)
            {
                Logging.Log.LogError("LoginUser: Wrong Configuration!");
                haserror = true;
            }
            else if (userPasswortHash == Constants.MsPassword)
            {
                Logging.Log.LogInfo("Login mit Microsoft Account");
            }
            else if (infos.RefreshToken == user.PasswordHash)
            {
                Logging.Log.LogInfo("Dc: MFa: ToDo");
            }
            else if (user.PasswordHash != userPasswortHash)
            {
                Logging.Log.LogWarning("LoginUser: Wrong Passwort!");
                haserror = true;
            }

            if (haserror)
            {
                return null!;
            }

            if (device!.TblUser == null || device.TblUser.Id != infos.UserId)
            {
                device.TblUser = user;
                user!.LastAccessAtUtc = DateTime.UtcNow;
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Logging.Log.LogError($"LoginUser Db SaveChanges Error: {e}");
                }
            }

            //MFa: Todo
            if (!String.IsNullOrEmpty(userPasswortHash))
            {
                infos.RefreshToken = userPasswortHash;
                infos.JwtToken = user.RestPassword;
            }

            return infos;
        }

        /// <summary>
        ///     Aktuellen Benutzer abmelden
        ///     Benuter Id aus der Device Tabelle entfernen
        /// </summary>
        /// <param name="deviceDeviceId">Device Id</param>
        /// <param name="deviceUserId">User Id</param>
        /// <returns>Ob erfolgreich</returns>
        public async Task<bool> LogoutUser(long deviceDeviceId, long deviceUserId)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);
            var user = await db.TblUsers.FirstOrDefaultAsync(u => u.Id == deviceUserId);
            var device = await db.TblDevices
                .Include(x => x.TblUser)
                .FirstOrDefaultAsync(d => d.Id == deviceDeviceId).ConfigureAwait(true);

            if (user == null || device == null)
            {
                Logging.Log.LogError("LoginUser: Wrong Configuration!");
                return false;
            }

            if (device.TblUser != null && device.TblUser.Id == deviceUserId)
            {
                user.LastAccessAtUtc = DateTime.UtcNow;
                device.TblUser = null;
                try
                {
                    await db.SaveChangesAsync().ConfigureAwait(true);
                }
                catch (Exception e)
                {
                    Logging.Log.LogError($"LoginUser Db SaveChanges Error: {e}");
                }
            }

            return true;
        }

        /// <summary>
        ///     Migration "alter" Apps
        /// </summary>
        /// <param name="oldInfos">Aktuelle Infos aus "ExUserAccountData"</param>
        /// <returns>Dc Core Infos</returns>
        public Task<ExDcCoreInfos> MigrateDevice(DcMigrationInfos oldInfos)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}