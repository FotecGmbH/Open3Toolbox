// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

namespace ExConfigExchange.Models.Dummies
{
    /// <summary>
    /// Diese Klasse ist nur für das Testen der <see cref="Services.ExConfigItemManager"/> da.
    /// </summary>{
    public class SubDummy1 : BaseDummy, ISubDummy
    {
        /// <summary>
        /// $DESCRIPTION$
        /// </summary>
        public string Dummy1sString { get; set; }

    }
}
