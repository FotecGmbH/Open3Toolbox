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
    /// Fields/Properties die als <see cref="interface"/> gelten (<see cref="interface"/> || <see cref="InterfaceAttribute"/> || (<see cref="ConfigureAsAttribute"/> && (<see cref="interface"/> || <see cref="InterfaceAttribute"/>))) 
    /// und mit diesem Attribute gekennzeichnet wurden, werden be Validierung als Invalid gelten, wenn noch keine Implementation gewählt wurde.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ImplementationRequiredAttribute : Attribute
    {
    }
}
