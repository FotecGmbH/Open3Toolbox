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
    ///     Konkrete Klassen die mit dieser Attribute gezeichnet wurden, werden wie Interfaces/Abstract Klassen interpretiert.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InterfaceAttribute : Attribute
    {
    }
}