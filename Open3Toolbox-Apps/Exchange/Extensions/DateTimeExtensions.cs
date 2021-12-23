// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System;

namespace Exchange.Extensions
{
    /// <summary>
    ///     <para>Extension für DateTime</para>
    ///     Klasse DateTimeExtensions. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Ersten Tag der Woche berechnen
        /// </summary>
        /// <param name="weekDay">Derzeitiger Wochentag</param>
        /// <returns>Erster Tag der Woche</returns>
        public static DateTime GetMonday(this DateTime weekDay)
        {
            var checkDate = weekDay.Date;

            while (checkDate.DayOfWeek != DayOfWeek.Monday)
            {
                checkDate = checkDate.AddDays(-1);
            }

            return checkDate;
        }

        /// <summary>
        ///     Ersten Tag des Monats berechnen
        /// </summary>
        /// <param name="day">Derzeitiger Tag</param>
        /// <returns>Erster Tag des Monats</returns>
        public static DateTime GetFirstDayInMonth(this DateTime day)
        {
            return new DateTime(day.Year, day.Month, 1);
        }
    }
}