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

using System;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    ///     Fields/Properties die mit diesem Attribute gekennzeichnet wurden, werden von der Benutzer nur lesbar sein, jedoch
    ///     nicht modifizierbar. <br />
    ///     <warning>Dies gilt für <b>Konkrete</b> Klassen referenzen nicht!</warning>
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReadOnlyAttribute : Attribute
    {
    }
}