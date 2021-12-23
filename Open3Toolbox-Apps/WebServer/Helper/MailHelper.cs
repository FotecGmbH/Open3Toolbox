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
using System.Threading.Tasks;
using Biss.AppConfiguration;
using Biss.Apps.Base;
using Biss.Log.Producer;
using Database.Context;
using Database.Tables;
using Exchange;
using Exchange.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RazorLight;
using WebExchange;
using WebServer.Controllers;

namespace WebServer.Helper
{
    /// <summary>
    ///     Helfer für das Arbeiten mit Mails
    /// </summary>
    public class MailHelper
    {
        /// <summary>
        ///     Sendet Bestätigungemail nach Registrierung
        /// </summary>
        /// <param name="user">Der Benutzer</param>
        /// <param name="deviceId">device Id von dem die Anforderung gekommen ist</param>
        /// <returns>Ob erfolgreich</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> SendValidationMail(TableUser user, long deviceId)
        {
            if (user == null)
            {
                return false;
            }

            // Notwendige Parameter werden gesetzt
            var bem = WebConstants.Email;
            var sender = "biss@fotec.at";
            var subject = "Bitte bestätigen Sie ihre Registrierung";
            var receiverBetaInfo = "";
            var ccReceifer = new List<string>();
            var receiver = user.LoginName;

            var url = $"{AppSettings.Current().SaApiHost}{nameof(WebLinksController.UserValidateEMail)}/{deviceId}/{user.Id}/{user.RestPassword}";
            Logging.Log.LogInfo($"SendCheckEMail: {url}");

            // Für Beta Versionen
            if (Constants.AppConfiguration.CurrentBuildType != EnumCurrentBuildType.CustomerRelease)
            {
                receiverBetaInfo = $"To: {receiver}, CC: ";
                foreach (var cc in ccReceifer)
                {
                    receiverBetaInfo += $"{cc}; ";
                }

                ccReceifer = null;
            }

            // Nötige Daten für die Darstellung werden zusammengesammelt
            var model = new ExNecessaryEmailData
                        {
                            Firstname = user.FirstName,
                            Lastname = user.LastName,
                            Link = url
                        };

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(AppContext.BaseDirectory)
                .UseMemoryCachingProvider()
                .Build();

            // HTML wird gerendert
            string htmlRendered = await engine.CompileRenderAsync("Views/EMail/EmailVerification", model);

            // Email wird gesendet
            var sendResult = await bem.SendHtmlEMail(sender, new List<string> {user.LoginName}, subject, htmlRendered + receiverBetaInfo, WebSettings.Current().SendEMailAsDisplayName, ccReceifer);
            return sendResult;
        }

        /// <summary>
        ///     Senden der Passwort-reset mail
        /// </summary>
        /// <param name="user">Der Benutzer</param>
        /// <returns>Ob erfolgreich</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> SendPasswordResetMail(TableUser user)
        {
            // Nötige Daten werden gesetzt
            var sender = "biss@fotec.at";
            var subject = "Setzen Sie Ihr Passwort zurück!";
            var receiverBetaInfo = "";
            var ccReceifer = new List<string>();
            var receiver = user.LoginName;
            Logging.Log.LogInfo($"UserStartResetPassword {user.Id}");


            if (user == null)
            {
                return false;
            }

            // PW Reset Url wird generiert
            var url = $"{AppSettings.Current().SaApiHost}{nameof(WebLinksController.UserResetPassword)}/{user.Id}/{user.RestPassword}";
            Logging.Log.LogInfo($"Send StartResetPassword: {url}");
            var bem = WebConstants.Email;

            // Für Darstellung nötige Daten werden zusammen gesammelt
            var model = new ExNecessaryEmailData {Firstname = user.FirstName, Lastname = user.LastName, Link = url};

            // HTML View wird gerendert
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(AppContext.BaseDirectory)
                .UseMemoryCachingProvider()
                .Build();

            // HTML wird gerendert
            string htmlRendered = await engine.CompileRenderAsync("Views/EMail/EmailPasswordReset", model);

            // Senden der Email
            var result = await bem.SendHtmlEMail(sender, new List<string> {user.LoginName}, subject, htmlRendered + receiverBetaInfo, WebSettings.Current().SendEMailAsDisplayName, ccReceifer);

            return result;
        }

        /// <summary>
        ///     Senden der Passwort-reset bestätigungsmail
        /// </summary>
        /// <param name="user">Der Benutzer</param>
        /// <returns>Ob erfolgreich</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<bool> SendPasswordResetConfirmationMail(TableUser user)
        {
            // Nötige Daten werden gesetzt
            var sender = "biss@fotec.at";
            var subject = "Passwort wurde geändert!";
            var receiverBetaInfo = "";
            var ccReceifer = new List<string>();
            var receiver = user.LoginName;
            Logging.Log.LogInfo($"UserStartResetPassword {user.Id}");
            if (user == null)
            {
                return false;
            }

            //Passwort wird in der Datenbankgeändert und gespeichert
            using (var db = new DatabaseContext(WebConstants.ConnectionString))
            {
                var data = await db.TblUsers.FirstOrDefaultAsync(u => u.Id == user.Id);
                var newPwd = AppCrypt.GeneratePassword(5);
                data.PasswordHash = AppCrypt.CumputeHash(newPwd);

                // TODO MFa: RestPasswort beim  User oder am Device?
                //data.RestPassword = AppCrypt.GeneratePassword();

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Logging.Log.LogError($"UserResetPassword: {e}");
                    return false;
                }
            }

            Logging.Log.LogInfo("Send UserResetPassword");

            // Benötigte Daten für die Darstellung werden zusammengesammelt 
            var bem = WebConstants.Email;
            var model = new ExNecessaryEmailData {Firstname = user.FirstName, Lastname = user.LastName, Password = user.RestPassword};

            // HTML View wird gerendert
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(AppContext.BaseDirectory)
                .UseMemoryCachingProvider()
                .Build();

            // HTML wird gerendert
            string htmlRendered = await engine.CompileRenderAsync("Views/EMail/EmailPasswordChanged", model);

            // Email wird gesendet
            var result = await bem.SendHtmlEMail(sender, new List<string> {user.LoginName}, subject, htmlRendered + receiverBetaInfo, WebSettings.Current().SendEMailAsDisplayName, ccReceifer);

            return result;
        }
    }
}