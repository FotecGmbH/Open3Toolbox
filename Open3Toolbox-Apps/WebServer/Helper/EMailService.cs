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
using Biss.Log.Producer;
using Database.Tables;
using WebServer.Services;

namespace WebServer.Helper
{
    /// <summary>
    ///     Handler for Emails
    /// </summary>
    public class EMailService
    {
        /// <summary>
        ///     View of the handler
        /// </summary>
        private readonly ViewRenderer _view;

        /// <summary>
        ///     Initializes a new instance
        /// </summary>
        /// <param name="view">The view of the handler</param>
        public EMailService(ViewRenderer view)
        {
            _view = view;
        }

        /// <summary>
        ///     Sending a test email
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>If succeeded</returns>
        public async Task<bool> SendTestEmail(TableUser user)
        {
            if (user != null)
            {
                MailHelper helper = new MailHelper();
                var result = await helper.SendValidationMail(user, -1);
                Logging.Log.LogInfo($"SendEMail Result: {result}");
            }

            return true;
        }
    }
}