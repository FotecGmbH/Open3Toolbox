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
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BaseApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Footer : ContentView
    {
        public Footer()
        {
            InitializeComponent();
        }
    }
}