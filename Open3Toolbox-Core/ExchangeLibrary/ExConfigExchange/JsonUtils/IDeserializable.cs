// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:50
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

namespace ExConfigExchange.JsonUtils
{
    /// <summary>
    ///     Mit dieser Interface + <see cref="InterfaceJsonConverter{T}" /> können interface Implementationen ohne den
    ///     konkreten Type zu kennen deserializiert werden.
    ///     <warning>Vergleiche den Type immer bevor deserializieren!</warning>
    ///     <warning>Hier muss aufgepasst werden sodass es keine Sicheheitslücke wird!</warning>
    /// </summary>
    public interface IDeserializable
    {
        #region Properties

        /// <summary>
        ///     Implement it wie: { get; } => GetType().FullName
        /// </summary>
        string TypeFullName { get; }

        /// <summary>
        ///     Implement it wie: { get; } => GetType().Assembly.FullName
        /// </summary>
        string AssemblyFullname { get; }

        #endregion
    }
}