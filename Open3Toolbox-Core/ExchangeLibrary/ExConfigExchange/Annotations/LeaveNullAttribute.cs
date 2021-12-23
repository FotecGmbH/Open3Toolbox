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
    ///     Fields/Properties die mit diesem Attribute gekennzeichnet wurden, werden null gelassen.
    ///     Außerdem werden diese Fields/Properties auch als <see cref="ReadOnlyAttribute" /> und
    ///     <see cref="HiddenAttribute" /> markiert.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LeaveNullAttribute : Attribute
    {
    }
}