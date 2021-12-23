// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       BaseApp.View
// 
// Released under MIT

using System;


namespace BaseApp.View
{
    public partial class ViewSettingsCommon
    {
        public ViewSettingsCommon() : this(null)
        {
            InitializeComponent();
        }

        public ViewSettingsCommon(object? args = null) : base(args)
        {
            InitializeComponent();
        }
    }
}