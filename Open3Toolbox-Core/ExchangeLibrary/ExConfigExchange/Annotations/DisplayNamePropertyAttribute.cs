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
using ExConfigExchange.Models;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    ///     Setzt sich als extra bei <see cref="ExObjectConfigItem.DisplayNameKey" /> zur Angezeigte Name
    ///     (DisplayName-DisplayKey(Übersetzt)).
    ///     Der Property's Value wird als der DisplayName verwendet.
    ///     Der erste Vorkommniss wird immer genommen.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNamePropertyAttribute : Attribute
    {
    }
}