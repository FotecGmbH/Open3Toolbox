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
using Database.Tables;

namespace WebServer.Helper
{
    /// <summary>
    ///     <para>Hilfe für die lokale Zeit eines User</para>
    ///     Klasse LocalTime. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public static class LocalTime
    {
        #region Properties

        /// <summary>
        ///     Get the current datetime
        /// </summary>
        private static DateTime UtcNow => new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0);

        #endregion

        /// <summary>
        ///     Get the current date time for a user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>The date time</returns>
        public static DateTime GetUserCurrentDateTime(TableUser? user)
        {
            if (user == null)
            {
                return UtcNow;
            }

            return GetUserCurrentDateTime(user.TimeDifference);
        }

        /// <summary>
        ///     Get the current date time, depending on a difference
        /// </summary>
        /// <param name="difference">The difference</param>
        /// <returns>The result date time</returns>
        public static DateTime GetUserCurrentDateTime(TimeSpan difference)
        {
            var dateTimeUtc = UtcNow;
            return dateTimeUtc.Add(difference);
        }
    }
}