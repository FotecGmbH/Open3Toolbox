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
using System.Collections.Generic;

namespace Exchange.Services.ConfigurationTool.Interfaces
{
    /// <summary>
    ///     MQTT Connector, zur Zeit unwichtig.
    /// </summary>
    [Obsolete("Zur Zeit wird nicht gebrauct.")]
    public interface IIdConnector
    {
        /// <summary>
        ///     Gets the uids.
        /// </summary>
        /// <returns></returns>
        List<string> GetUids();

        /// <summary>
        ///     Sends the pairs.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        void SendPairs(Dictionary<string, long> pairs);
    }
}