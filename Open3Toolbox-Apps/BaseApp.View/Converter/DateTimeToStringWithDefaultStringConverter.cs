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
using Xamarin.Forms;

namespace BaseApp.Converter
{
    /// <summary>
    ///     <para>
    ///         Konvertiert ein DateTime zu einem String (in lokaler Zeit) mit optionaler Angabe eines Default-Wertes wenn
    ///         DateTime null.
    ///     </para>
    ///     Klasse DateTimeToStringWithDefaultStringConverter. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class DateTimeToStringWithDefaultStringConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        ///     Nur Datum anzeigen
        /// </summary>
        public bool DateOnly { get; set; }

        #endregion

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
            if (value is null)
            {
                return parameter;
            }

            if (value is DateTime dt)
            {
                if (DateOnly)
                {
                    return dt.ToString("d");
                }

                return dt.ToShortTimeString();
            }

            return string.Empty;
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