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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BaseApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Header : ContentView
    {
        public static readonly BindableProperty ShowBackProperty = BindableProperty.Create(
            nameof(ShowBack), // the name of the bindable property
            typeof(bool), // the bindable property type
            typeof(Header), // the parent object type
            default(bool) // the default value for the property
        );

        public static readonly BindableProperty ShowTodoProperty = BindableProperty.Create(
            nameof(ShowTodo), // the name of the bindable property
            typeof(bool), // the bindable property type
            typeof(Header), // the parent object type
            default(bool) // the default value for the property
        );

        public static readonly BindableProperty ShowTeamsProperty = BindableProperty.Create(
            nameof(ShowTeams), // the name of the bindable property
            typeof(bool), // the bindable property type
            typeof(Header), // the parent object type
            true // the default value for the property
        );

        public Header()
        {
            InitializeComponent();
        }

        #region Properties

        /// <summary>
        ///     Zurück Button anzeigen
        /// </summary>
        public bool ShowBack
        {
            get => (bool) GetValue(ShowBackProperty);
            set => SetValue(ShowBackProperty, value);
        }

        /// <summary>
        ///     To do statt Teams Button anzeigen
        /// </summary>
        public bool ShowTodo
        {
            get => (bool) GetValue(ShowTodoProperty);
            set => SetValue(ShowTodoProperty, value);
        }

        /// <summary>
        ///     Teams Button anzeigen
        /// </summary>
        public bool ShowTeams
        {
            get => (bool) GetValue(ShowTeamsProperty);
            set => SetValue(ShowTeamsProperty, value);
        }

        #endregion
    }
}