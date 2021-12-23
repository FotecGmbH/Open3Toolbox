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
using System.Globalization;
using Exchange.Extensions;
using Xamarin.Forms;

namespace BaseApp.Converter
{
    /// <summary>
    ///     <para>Double Wert für UI als Stunden anzeigen</para>
    ///     Klasse DoubleToHoursConverter. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class DoubleToHoursConverter : IValueConverter
    {
        #region Interface Implementations

        /// <summary>
        ///     Konvertiert ein Objekt für XAML
        /// </summary>
        /// <param name="value">Wert zum konvertieren für das UI</param>
        /// <param name="targetType">Zieltyp des Werts</param>
        /// <param name="parameter">Zusätzlicher Parameter aus XAML</param>
        /// <param name="culture">Aktuelle Kultur</param>
        /// <returns>Konvertierter Wert oder null</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan span)
            {
                return span.ToUiTime();
            }

            if (value is double dHours)
            {
                var ts = TimeSpan.FromHours(dHours);
                return ts.ToUiTime();
            }

            if (value is float fHours)
            {
                var ts = TimeSpan.FromHours(fHours);
                return ts.ToUiTime();
            }

            return null;
        }

        /// <summary>
        ///     Konvertiert ein Objekt von XAML
        /// </summary>
        /// <param name="value">Wert zum konvertieren für das Datenobjekt</param>
        /// <param name="targetType">Zieltyp des Werts</param>
        /// <param name="parameter">Zusätzlicher Parameter aus XAML</param>
        /// <param name="culture">Aktuelle Kultur</param>
        /// <returns>Konvertierter Wert oder UnsetValue</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        #endregion
    }
}