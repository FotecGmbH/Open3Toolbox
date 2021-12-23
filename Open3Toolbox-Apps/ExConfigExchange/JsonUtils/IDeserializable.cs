// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

namespace ExConfigExchange.JsonUtils
{
    /// <summary>
    /// Mit dieser Interface + <see cref="InterfaceJsonConverter{T}"/> können interface Implementationen ohne den konkreten Type zu kennen deserializiert werden.
    /// <warning>Vergleiche den Type immer bevor deserializieren!</warning>
    /// <warning>Hier muss aufgepasst werden sodass es keine Sicheheitslücke wird!</warning>
    /// </summary>
    public interface IDeserializable
    {
        /// <summary>
        /// Implement it wie: { get; } => <see cref="this.GetType().FullName"/>
        /// </summary>
        string TypeFullName { get; }

        /// <summary>
        /// Implement it wie: { get; } => <see cref="this.GetType().Assembly.FullName"/>
        /// </summary>
        string AssemblyFullname { get; }
    }
}