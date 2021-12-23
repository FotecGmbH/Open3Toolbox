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
    ///     Fields/Properties die alsinterface ||
    ///     <see cref="InterfaceAttribute" /> || (<see cref="ConfigureAsAttribute" /> && interface ||
    ///     <see cref="InterfaceAttribute" />)))
    ///     und mit diesem Attribute gekennzeichnet wurden, werden be Validierung als Invalid gelten, wenn noch keine
    ///     Implementation gewählt wurde.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ImplementationRequiredAttribute : Attribute
    {
    }
}