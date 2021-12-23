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
    public partial class FooterButton : ContentView
    {
        public static readonly BindableProperty BissCommandProperty = BindableProperty.Create(
            nameof(BissCommand), // the name of the bindable property
            typeof(VmCommandSelectable), // the bindable property type
            typeof(FooterButton)); // the default value for the property

        public static readonly BindableProperty IsSelectedVisibleProperty = BindableProperty.Create(
            nameof(IsSelectedVisible), // the name of the bindable property
            typeof(bool), // the bindable property type
            typeof(FooterButton), // the parent object type
            true); // the default value for the property

        public FooterButton()
        {
            InitializeComponent();
        }

        #region Properties

        public VmCommandSelectable BissCommand
        {
            get => (VmCommandSelectable) GetValue(BissCommandProperty);
            set => SetValue(BissCommandProperty, value);
        }

        public bool IsSelectedVisible
        {
            get => (bool) GetValue(IsSelectedVisibleProperty);
            set => SetValue(IsSelectedVisibleProperty, value);
        }

        #endregion
    }
}