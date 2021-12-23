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
    public partial class FirstLoginItem : ContentView
    {
        public static readonly BindableProperty IsDoneProperty = BindableProperty.Create(
            nameof(IsDone), // the name of the bindable property
            typeof(bool), // the bindable property type
            typeof(FirstLoginItem), // the parent object type
            false); // the default value for the property

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
            nameof(TitleText), // the name of the bindable property
            typeof(string), // the bindable property type
            typeof(FirstLoginItem), // the parent object type
            string.Empty); // the default value for the property

        public static readonly BindableProperty ContentTextProperty = BindableProperty.Create(
            nameof(ContentText), // the name of the bindable property
            typeof(string), // the bindable property type
            typeof(FirstLoginItem), // the parent object type
            string.Empty); // the default value for the property

        public static readonly BindableProperty ShowCommandProperty = BindableProperty.Create(
            nameof(ShowCommand), // the name of the bindable property
            typeof(VmCommandSelectable), // the bindable property type
            typeof(FirstLoginItem)); // the default value for the property

        public FirstLoginItem()
        {
            InitializeComponent();
        }

        #region Properties

        public bool IsDone
        {
            get => (bool) GetValue(IsDoneProperty);
            set => SetValue(IsDoneProperty, value);
        }

        public string TitleText
        {
            get => (string) GetValue(TitleTextProperty);
            set => SetValue(TitleTextProperty, value);
        }

        public string ContentText
        {
            get => (string) GetValue(ContentTextProperty);
            set => SetValue(ContentTextProperty, value);
        }

        public VmCommandSelectable ShowCommand
        {
            get => (VmCommandSelectable) GetValue(ShowCommandProperty);
            set => SetValue(ShowCommandProperty, value);
        }

        #endregion
    }
}