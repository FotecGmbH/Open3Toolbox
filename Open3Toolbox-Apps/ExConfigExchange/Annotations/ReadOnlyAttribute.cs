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
    /// Fields/Properties die mit diesem Attribute gekennzeichnet wurden, werden von der Benutzer nur lesbar sein, jedoch nicht modifizierbar. <br/>
    /// <warning>Dies gilt für <b>Konkrete</b> Klassen referenzen nicht!</warning>
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReadOnlyAttribute : Attribute
    {
    }
}
