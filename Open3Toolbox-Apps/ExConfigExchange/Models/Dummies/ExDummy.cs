// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using System.Collections.Generic;
using Biss.Interfaces;
using ExConfigExchange.Annotations;

namespace ExConfigExchange.Models.Dummies
{
    /// <summary>
    /// Diese Klasse ist nur für das Testen der <see cref="Services.ExConfigItemManager"/> da.
    /// </summary>
    public class ExDummy : IBissSerialize
    {
        [ConfigureAs(typeof(SubDummy2))]
        public string ConfigureAsTest { get; set; }
        
        public Uri MyUri { get; set; }
        public string MyString { get; set; }
        public int MyInt { get; set; }
        public float MyFloat { get; set; }
        public double MyDouble { get; set; }
        public byte MyByte { get; set; }
        public bool MyBool { get; set; }

        [ReadOnly]
        public ConsoleColor MyColor { get; set; } = ConsoleColor.DarkRed;

        public BaseDummy MyFakeInterface { get; set; }
        
        public ISubDummy MyInterface { get; set; }
        
        public SubDummy1 MyObject { get; set; }
        
        public List<string> MyBasicList1 { get; set; }
        public List<ISubDummy> MyInterfaceList1 { get; set; }
        public List<SubDummy2> MyObjectList1 { get; set; }

        [Hidden]
        public SubDummy2 MyHidden { get; set; }

        [LeaveNull]
        public SubDummy1 MyLeaveNull { get; set; }
    }
}
