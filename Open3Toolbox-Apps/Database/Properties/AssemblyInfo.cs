// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       DatabaseTables
// 
// Released under MIT

using System.Reflection;

// Version der Assemblies:
//
//      A: Major Version   =>  Hauptreleasezweig. Erhöhnung bei Neueentwicklung
//      B: Minor Version   =>  Nebenzeig. Erhöhung wenn sich die App Funktionell geändert hat (Neue Funktionen oder Funktionen entfernt)
//      C: Build Number    =>  Build. Erhöhng bei Bugfixing, bestehende Funktionen minimal erweitert, Designänderrungen
//      D: Revision        =>  Relese/Beta Zweig: BETA/DEVELOPER Versionen wird diese Nummer erhöht. 
//                             Bei neuer CUSTOMER_RELESE muss diese Nummer auf 0 sein und dafür die Build/Minor erhöht werden
//
//      
//      Einzelne Nummern Min: 0     Max: 255
//      Build Integer (Android) ist wie folgt Codiert: AABBCCDD (Jede einzelne Nummber ist dabei HEX Codiert) zB: 1.10.0.11 wird HEX 010A000B (17 432 587)
//
//
// * für Build bzw. Revision sollte nicht verwendet werden.
[assembly: AssemblyVersion("0.5.0.0")]
[assembly: AssemblyFileVersion("0.5.0.0")]

// Metainformationen der Assemblies:
[assembly: AssemblyDescription("Open3 Toolbox")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("FOTEC Forschungs- und Technologietransfer GmbH")]
[assembly: AssemblyProduct("Open3 Toolbox")]
[assembly: AssemblyCopyright("Copyright © 1998-2021 FOTEC - Forschungs- und Technologietransfer GmbH")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]