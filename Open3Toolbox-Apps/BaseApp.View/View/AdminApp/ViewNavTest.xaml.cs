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
using Xamarin.Forms.Xaml;

// ReSharper disable once CheckNamespace
namespace BaseApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewNavTest
    {
        public ViewNavTest()
        {
            InitializeComponent();
        }

        public ViewNavTest(object args = null) : base(args)
        {
            InitializeComponent();
        }
    }
}