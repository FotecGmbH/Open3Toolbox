// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Erstversion vom 23.12.2021 12:29
// Entwickler      Manuel Fasching
// Projekt         DataVisualisation
//
// Released under MIT

using System.Collections.Generic;

namespace DataVisualisation
{
    /// <summary>
    /// Menüeintrag
    /// </summary>
    public class MenuEntry
    {
        #region Properties

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Expanded { get; set; }
        public IEnumerable<MenuEntry> Children { get; set; }
        public IEnumerable<string> Tags { get; set; }

        #endregion
    }
}