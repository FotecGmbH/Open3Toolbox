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

namespace BaseApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewLogin
    {
        public ViewLogin() : this(null)
        {
        }

        public ViewLogin(object? args = null) : base(args)
        {
            InitializeComponent();
        }
    }
}