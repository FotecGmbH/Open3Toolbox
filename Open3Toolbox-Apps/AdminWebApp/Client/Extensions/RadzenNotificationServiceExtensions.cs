// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       30.08.2021 15:55
// Developer:     Istvan Galfi
// Project:       AdminWebApp.Client
// 
// Released under MIT

using System;
using Radzen;

namespace AdminWebApp.Client.Extensions
{
    public static class RadzenNotificationServiceExtensions
    {
        /// <summary>
        ///     Notifies when a service succeeded
        /// </summary>
        /// <param name="notificationService">The notification service</param>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        public static void NotifyOfSuccess(this NotificationService notificationService, string title, string message)
        {
            notificationService.Notify(
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = title,
                    Detail = message,
                    Duration = 4000
                }
            );
        }

        /// <summary>
        ///     Notifies when a service fails
        /// </summary>
        /// <param name="notificationService">The notification service</param>
        /// <param name="title">The title</param>
        /// <param name="message">The message</param>
        public static void NotifyOfFailure(this NotificationService notificationService, string title, string message)
        {
            notificationService.Notify(
                new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = title,
                    Detail = message,
                    Duration = 4000
                }
            );
        }
    }
}