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
    ///     <para>Erweiterungen für Timespan</para>
    ///     Klasse TimeSpanExtensions. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        ///     Parses a timespan to a printable UI time
        /// </summary>
        /// <param name="ts">The timespan</param>
        /// <returns>The printable UI time</returns>
        public static string ToUiTime(this TimeSpan ts)
        {
            if (ts == TimeSpan.Zero)
            {
                return "-";
            }

            return (ts.TotalHours > 0 ? $" {(int) ts.TotalHours}h" : string.Empty) +
                   (ts.Minutes > 0 ? $" {ts.Minutes}m" : string.Empty) +
                   (ts.Seconds > 0 ? $" {ts.Seconds}s" : string.Empty);
        }
    }
}