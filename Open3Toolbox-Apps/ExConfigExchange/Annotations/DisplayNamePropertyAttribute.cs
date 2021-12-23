// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using ExConfigExchange.Models;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    /// Setzt sich als extra bei <see cref="ExObjectConfigItem.DisplayNameKey"/> zur Angezeigte Name (DisplayName-DisplayKey(Übersetzt)).
    /// Der Property's Value wird als der DisplayName verwendet.
    /// Der erste Vorkommniss wird immer genommen.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayNamePropertyAttribute : Attribute
    {
    }
}
