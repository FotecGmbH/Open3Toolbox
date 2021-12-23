// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Threading.Tasks;

namespace ExchangeLibrary
{
    /// <summary>
    ///     Responsible for the connection, which gets the id.
    ///     Start async should start steps to get the wright id
    /// </summary>
    public interface IConnectionForId
    {
        /// <summary>
        ///     Start steps to get the wright id for this device
        /// </summary>
        /// <param name="idReceivedCommand">The command which will get executed, when an id arrives</param>
        /// <returns>A Task</returns>
        public Task StartAsync(ICommand idReceivedCommand);
    }
}