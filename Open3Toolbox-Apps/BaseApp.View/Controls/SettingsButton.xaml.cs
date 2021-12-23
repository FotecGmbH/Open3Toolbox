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
using Biss.Apps.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BaseApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsButton : ContentView
    {
        public static readonly BindableProperty BissCommandProperty = BindableProperty.Create(
            nameof(BissCommand), // the name of the bindable property
            typeof(VmCommandSelectable), // the bindable property type
            typeof(SettingsButton)); // the default value for the property


        public SettingsButton()
        {
            InitializeComponent();
        }

        #region Properties

        public VmCommandSelectable BissCommand
        {
            get => (VmCommandSelectable) GetValue(BissCommandProperty);
            set => SetValue(BissCommandProperty, value);
        }

        public bool ShowMsTeamsLogo { get; set; }

        #endregion
    }
}