// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    /// Fields/Properties die mit diesem Attribute gekennzeichnet wurden, werden <see cref="null"/> gelassen.
    /// Außerdem werden diese Fields/Properties auch als <see cref="ReadOnlyAttribute"/> und <see cref="HiddenAttribute"/> markiert.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LeaveNullAttribute : Attribute
    {
    }
}
