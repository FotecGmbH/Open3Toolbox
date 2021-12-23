// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       AdminWebApp.Client
// 
// Released under MIT

using System;

namespace AdminWebApp.Client.Shared
{
    public class DrawerItem
    {
        #region Properties

        public string Text { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        public string Group { get; set; }

        #endregion
    }
}