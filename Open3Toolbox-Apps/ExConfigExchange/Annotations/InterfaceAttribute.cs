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
    /// Konkrete Klassen die mit dieser Attribute gezeichnet wurden, werden wie Interfaces/Abstract Klassen interpretiert.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InterfaceAttribute : Attribute
    {
    }
}
