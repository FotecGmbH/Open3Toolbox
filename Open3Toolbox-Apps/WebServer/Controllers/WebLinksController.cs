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
using Biss.Apps.Base;
using Biss.Log.Producer;
using Database.Context;
using Database.Tables;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebExchange;
using WebServer.DataConnector;
using WebServer.Helper;

namespace WebServer.Controllers
{
    /// <summary>
    ///     <para>Controller für Aktionen via Web-Links</para>
    ///     Klasse RegistrationController. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    [ApiController]
    public class WebLinksController : ControllerBase
    {
        /// <summary>
        ///     Die server remote calls
        /// </summary>
        private readonly ServerRemoteCalls _hub;

        /// <summary>
        ///     Initialisiert eine neue Instanz
        /// </summary>
        /// <param name="calls"></param>
        public WebLinksController(IServerRemoteCalls calls)
        {
            _hub = (ServerRemoteCalls) calls;
        }

        /// <summary>
        ///     EMail gedrückt zum Bestätigen des Users
        /// </summary>
        /// <param name="deviceId">Device Id (Datenbank)</param>
        /// <param name="userId">User Id (Datenbank)</param>
        /// <param name="token">token</param>
        /// <returns>Ob valide oder nicht</returns>
        [AllowAnonymous]
        [Route("api/UserValidateEMail/{deviceId}/{userId}/{token}")]
        [HttpGet]
        public async Task<bool> UserValidateEMail(long deviceId, long userId, string token)
        {
            Logging.Log.LogInfo($"UserValidateEMail {deviceId},{userId},{token}");
            await using var db = new DatabaseContext(WebConstants.ConnectionString);
            var data = await db.TblUsers.FirstOrDefaultAsync(u => u.Id == userId);

            if (data == null)
            {
                return false;
            }

            if (data.LoginConfirmed)
            {
                return true;
            }

            if (token != data.RestPassword)
            {
                return false;
            }

            data.LoginConfirmed = true;
            data.RestPassword = AppCrypt.GeneratePassword();

            //Könnte auch später passieren in einem WebFrontend zb (Beispiel smartflower)
            data.Locked = false;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logging.Log.LogError($"ValidateEMail: {e}");
                return false;
            }

            if (deviceId > 0)
            {
                //var d1 = await _hub.GetDcExCheckUserFromDb(db, deviceId, data.Id);
                //var r1 = await _hub.SendDcExCheckUser(d1, deviceId);
            }

            return true;
        }

        /// <summary>
        ///     EMail gedrückt zum Rücksetzen des User Passworts
        /// </summary>
        /// <param name="userId">User Id (Datenbank)</param>
        /// <param name="token">token</param>
        /// <returns>Ob ja oder nein</returns>
        [AllowAnonymous]
        [Route("api/UserResetPassword/{userId}/{token}")]
        [HttpGet]
        public async Task<bool> UserResetPassword(int userId, string token)
        {
            TableUser? user = null;
            Logging.Log.LogInfo($"UserResetPassword {userId},{token}");

            await using var db = new DatabaseContext(WebConstants.ConnectionString);
            user = await db.TblUsers.Where(t => userId == t.Id).FirstOrDefaultAsync();

            if (user == null)
            {
                return false;
            }

            if (token != user.RestPassword)
            {
                return false;
            }

            MailHelper helper = new MailHelper();
            var endresult = await helper.SendPasswordResetConfirmationMail(user);
            return endresult;
        }
    }
}