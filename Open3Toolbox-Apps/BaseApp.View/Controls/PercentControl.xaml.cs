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
using BaseApp.Styles;
using Biss.Log.Producer;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BaseApp.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PercentControl
    {
        public static readonly BindableProperty ValueProperty = BindableProperty.Create(
            nameof(Value), // the name of the bindable property
            typeof(double), // the bindable property type
            typeof(PercentControl), // the parent object type
            default(double), // the default value for the property
            propertyChanged: ValuePropertyChanged
        );

        public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
            nameof(MaxValue), // the name of the bindable property
            typeof(double), // the bindable property type
            typeof(PercentControl), // the parent object type
            1d, // the default value for the property
            propertyChanged: MaxValuePropertyChanged
        );

        public static readonly BindableProperty LineHeightProperty = BindableProperty.Create(
            nameof(LineHeight), // the name of the bindable property
            typeof(double?), // the bindable property type
            typeof(PercentControl), // the parent object type
            default(double?), // the default value for the property
            propertyChanged: LineHeightPropertyChanged
        );

        public PercentControl()
        {
            InitializeComponent();
        }

        #region Properties

        /// <summary>
        ///     Höhe der Linie
        /// </summary>
        public double? LineHeight
        {
            get => GetValue(LineHeightProperty) as double?;
            set => SetValue(LineHeightProperty, value);
        }

        /// <summary>
        ///     Value für UI
        /// </summary>
        public double Value
        {
            get => (double) GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        ///     MaxValue für UI
        /// </summary>
        public double MaxValue
        {
            get => (double) GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        ///     Minimum UI
        /// </summary>
        public double MinValueUi { get; set; }

        /// <summary>
        ///     Maximum UI
        /// </summary>
        public double MaxValueUi { get; set; }

        #endregion

        /// <summary>
        ///     Timeline neu zeichnen
        /// </summary>
        public void RedrawGrid()
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
            {
                if (MainThread.IsMainThread)
                {
                    RedrawGridInternal();
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(RedrawGridInternal);
                }
            }
            else
            {
                Dispatcher.BeginInvokeOnMainThread(RedrawGridInternal);
            }
        }

        /// <summary>
        ///     Wert gesetzt
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void ValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PercentControl pc)
            {
                pc.RedrawGrid();
            }
        }

        /// <summary>
        ///     Maximalwert geändert
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void MaxValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PercentControl pc)
            {
                pc.RedrawGrid();
            }
        }

        /// <summary>
        ///     Höhe für die Timeline wurde gesetzt
        /// </summary>
        /// <param name="bindable">UI</param>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        private static void LineHeightPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PercentControl pc)
            {
                pc.RedrawGrid();
            }
        }

        private void RedrawGridInternal()
        {
            try
            {
                LineGrid.Children.Clear();
                LineGrid.ColumnDefinitions.Clear();
                LineGrid.RowDefinitions.Clear();

                // Add Rows
                LineGrid.RowDefinitions.Add(new RowDefinition
                                            {
                                                Height = LineHeight.HasValue ? new GridLength(LineHeight.Value) : GridLength.Star
                                            });

                MinValueUi = 0;
                MaxValueUi = MaxValue > Value || (Value <= 0 || double.IsNaN(Value) || double.IsInfinity(Value)) ? MaxValue : Value;

                var styColors = new StyColors();

                if (Value <= 0 || double.IsNaN(Value) || double.IsInfinity(Value) || double.IsNaN(MaxValue) || double.IsInfinity(MaxValue))
                {
                    LineGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star});

                    var box = new BoxView
                              {
                                  Color = (Color) styColors["PercentControlMax"],
                              };

                    Grid.SetColumn(box, 0);
                    LineGrid.Children.Add(box);
                }
                else if (MaxValue <= 0)
                {
                    LineGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star});

                    var box = new BoxView
                              {
                                  Color = (Color) styColors["PercentControlOver"],
                              };

                    Grid.SetColumn(box, 0);
                    LineGrid.Children.Add(box);
                }
                else if (Math.Abs(Value - MaxValue) <= 0.0001)
                {
                    LineGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = GridLength.Star});

                    var box = new BoxView
                              {
                                  Color = (Color) styColors["PercentControlMin"],
                              };

                    Grid.SetColumn(box, 0);
                    LineGrid.Children.Add(box);
                }
                else if (Value < MaxValue)
                {
                    var left = Value / MaxValue;
                    var right = (MaxValue - Value) / MaxValue;

                    LineGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(left, GridUnitType.Star)});
                    LineGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(right, GridUnitType.Star)});

                    var leftBox = new BoxView
                                  {
                                      Color = (Color) styColors["PercentControlMin"],
                                  };
                    var rightBox = new BoxView
                                   {
                                       Color = (Color) styColors["PercentControlMax"],
                                   };

                    Grid.SetColumn(leftBox, 0);
                    Grid.SetColumn(rightBox, 1);

                    LineGrid.Children.Add(leftBox);
                    LineGrid.Children.Add(rightBox);
                }
                else
                {
                    var left = MaxValue / Value;
                    var right = (Value - MaxValue) / Value;

                    LineGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(left, GridUnitType.Star)});
                    LineGrid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(right, GridUnitType.Star)});

                    var leftBox = new BoxView
                                  {
                                      Color = (Color) styColors["PercentControlMin"],
                                  };
                    var rightBox = new BoxView
                                   {
                                       Color = (Color) styColors["PercentControlOver"],
                                   };

                    Grid.SetColumn(leftBox, 0);
                    Grid.SetColumn(rightBox, 1);

                    LineGrid.Children.Add(leftBox);
                    LineGrid.Children.Add(rightBox);
                }
            }
            catch (Exception e)
            {
                Logging.Log.LogError($"{e}");
            }
        }
    }
}