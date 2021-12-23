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
using Exchange.Services.ConfigurationTool.Interfaces;

namespace Exchange.Services.ConfigurationTool
{
    /// <summary>
    ///     MQTT Connector Dummy, zur Zeit unwichtig.
    /// </summary>
    /// <seealso cref="Exchange.Services.ConfigurationTool.Interfaces.IIdConnector" />
    [Obsolete("Is currently Not in use")]
    public class DummyIdConnector : IIdConnector
    {
        #region Interface Implementations

        /// <summary>
        ///     Gets the uids.
        /// </summary>
        /// <returns></returns>
        public List<string> GetUids()
        {
            return new List<string>
                   {
                       Guid.NewGuid().ToString(),
                       Guid.NewGuid().ToString(),
                       Guid.NewGuid().ToString(),
                       Guid.NewGuid().ToString(),
                       Guid.NewGuid().ToString(),
                       Guid.NewGuid().ToString(),
                       Guid.NewGuid().ToString(),
                       Guid.NewGuid().ToString(),
                       Guid.NewGuid().ToString(),
                   };
        }

        /// <summary>
        ///     Sends the pairs.
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        public void SendPairs(Dictionary<string, long> pairs)
        {
        }

        #endregion
    }
}