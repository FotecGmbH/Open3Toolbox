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
    /// Fields/Properties die mit diesem Attribute gekennzeichnet wurden, werden vor dem Benutzer versteckt und können daher von ihm nicht konfiguriert werden.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HiddenAttribute : Attribute
    {
    }
}
