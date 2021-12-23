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
using System.Collections.Generic;
using System.Linq;
using Telerik.XamarinForms.Common;
using Telerik.XamarinForms.Input;
using Telerik.XamarinForms.Input.Calendar;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BaseApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarControl
    {
        public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(
            nameof(SelectedDate), // the name of the bindable property
            typeof(DateTime?), // the bindable property type
            typeof(CalendarControl), // the parent object type
            default(DateTime?), // the default value for the property
            BindingMode.TwoWay,
            propertyChanged: SelectedDatePropertyChanged
        );

        public static readonly BindableProperty DisplayDateProperty = BindableProperty.Create(
            nameof(DisplayDate), // the name of the bindable property
            typeof(DateTime?), // the bindable property type
            typeof(CalendarControl), // the parent object type
            default(DateTime?), // the default value for the property
            BindingMode.TwoWay
        );

        /// <summary>
        ///     Kalender Control
        /// </summary>
        public CalendarControl()
        {
            InitializeComponent();
        }

        #region Properties

        /// <summary>
        ///     gewählter Arbeitstag zum Anzeigen/Editieren
        /// </summary>
        public DateTime? SelectedDate
        {
            get => GetValue(SelectedDateProperty) as DateTime?;
            set => SetValue(SelectedDateProperty, value);
        }

        /// <summary>
        ///     Datum der Anzeige - wochenweise immer vom aktuellen Wochentag
        /// </summary>
        public DateTime? DisplayDate
        {
            get => GetValue(DisplayDateProperty) as DateTime?;
            set => SetValue(DisplayDateProperty, value);
        }

        /// <summary>
        ///     Termine für Kalender -> Farbliche Punkte unter dem Datum
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
#pragma warning disable CA1002 // Do not expose generic lists
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
#pragma warning restore CA1002 // Do not expose generic lists
#pragma warning restore CA2227 // Collection properties should be read only

        #endregion

        /// <summary>
        ///     Gewählter Tag
        /// </summary>
        /// <param name="bindable">UI</param>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        private static void SelectedDatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CalendarControl dtc)
            {
                if (newValue is DateTime wd)
                {
                    if (dtc.RadCal != null && dtc.RadCal.SelectedDate != wd.Date.Date)
                    {
                        dtc.RadCal.SelectedDate = wd.Date.Date;
                        dtc.RadCal.DisplayDate = wd.Date.Date;
                    }
                }
                else if (newValue == null!)
                {
                    if (dtc.RadCal != null && dtc.RadCal.SelectedDate != null)
                    {
                        dtc.RadCal.SelectedDate = null; //dtc.DisplayDate;
                    }
                }
            }
        }

        #region UI Kalender Changes

        /// <summary>
        ///     beim Kalender wurde ein Datum gewählt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadCalendar_OnSelectionChanged(object sender, CalendarSelectionChangedEventArgs<object> e)
        {
            if (e.AddedItems is List<DateTime> selectedItems)
            {
                var value = selectedItems.Any() ? (DateTime?) selectedItems.FirstOrDefault() : null;
                SelectedDate = value?.Date;
            }
        }

        /// <summary>
        ///     beim Kalender wird ein anderes Datum gezeigt
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadCalendar_OnDisplayDateChanged(object sender, ValueChangedEventArgs<object> e)
        {
            if (e.NewValue is DateTime displayDate)
            {
                DisplayDate = displayDate.Date;
            }
            else if (e.NewValue == null)
            {
                DisplayDate = null;
            }
        }

        #endregion
    }
}