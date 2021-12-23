// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using Biss.Interfaces;
using ExConfigExchange.Annotations;

namespace ExConfigExchange.Models.Dummies
{
    /// <summary>
    /// Diese Klasse ist nur für das Testen der <see cref="Services.ExConfigItemManager"/> da.
    /// </summary>
    public class SubDummy2 : ISubDummy, IBissSerialize
    {
        [ValidRange(0,1,0.001)]
        public byte Dummy2sByte { get; set; }
    }
}
