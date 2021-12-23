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
        ///     Device fordert Daten für DcExUserSettings
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <returns>Daten oder eine Exception auslösen</returns>
        public async Task<ExUser> GetDcExUserSettings(long deviceId, long userId)
        {
            if (userId <= 0)
            {
                throw new Exception("GetDcExUserSettings userId is not valid!");
            }

            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            var user = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);

            return user;
        }

        /// <summary>
        ///     Device will Daten für DcExNewUserData sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <returns>Ergebnis (bzw. Infos zum Fehler)</returns>
        public async Task<DcStoreResult> SetDcExUserSettings(long deviceId, long userId, ExUser data)
        {
            await using var db = new DatabaseContext(WebConstants.ConnectionString);

            var user = await db.TblUsers.FirstOrDefaultAsync(x => x.Id == userId).ConfigureAwait(false);

            UpdateUserSettings(user, data);

            await db.SaveChangesAsync().ConfigureAwait(false);

            return new DcStoreResult();
        }

        private void UpdateUserSettings(ExUser item, ExUser newData)
        {
            // update settings
            item.AutoWorkStart = newData.AutoWorkStart;
            item.AutoWorkEnd = newData.AutoWorkEnd;
            item.FirstAidActive = newData.FirstAidActive;
            item.MsTodoListActive = newData.MsTodoListActive;
            item.MsTodoListName = newData.MsTodoListName;
            item.PushNotificationOnFocusEnd = newData.PushNotificationOnFocusEnd;
            item.PushNotificationOnPauseEnd = newData.PushNotificationOnPauseEnd;
            item.PushNotificationOnPauseReminder = newData.PushNotificationOnPauseReminder;
            item.PushNotificationOnWorkStart = newData.PushNotificationOnWorkStart;
            item.PushNotificationOnWorkEnd = newData.PushNotificationOnWorkEnd;
            item.PushNotificationOnWorkInFreetime = newData.PushNotificationOnWorkInFreetime;
            item.Accuracy = newData.Accuracy;
            item.FirstSurveyLinkClicked = newData.FirstSurveyLinkClicked;
            item.SecondSurveyLinkClicked = newData.SecondSurveyLinkClicked;
            item.ShowInfoTexts = newData.ShowInfoTexts;
            item.WorkTemplatesAllDays = newData.WorkTemplatesAllDays;
            item.FocusTimeUiName = newData.FocusTimeUiName;
        }
    }
}