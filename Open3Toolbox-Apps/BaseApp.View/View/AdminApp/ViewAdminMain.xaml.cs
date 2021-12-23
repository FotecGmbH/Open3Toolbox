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
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// ReSharper disable once CheckNamespace
namespace BaseApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAdminMain
    {
        public ViewAdminMain()
        {
            InitializeComponent();
        }

        public ViewAdminMain(object args = null) : base(args)
        {
            InitializeComponent();
            if (Navigation.NavigationStack.Count > 0)
            {
                ClearStackPanelAndNavigateFrist(Navigation);
            }
        }

        public void ClearStackPanelAndNavigateFrist(INavigation navigation)
        {
            var existingPages = navigation.NavigationStack.ToList();
            foreach (var t in existingPages)
            {
                navigation.RemovePage(t);
            }
        }
    }
}