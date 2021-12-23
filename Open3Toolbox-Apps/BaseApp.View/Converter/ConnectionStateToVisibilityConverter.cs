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
using Biss.Dc.Core;
using Xamarin.Forms;

namespace BaseApp.Converter
{
    /// <summary>
    ///     <para>DC Connection State To Visibility</para>
    ///     Klasse ConnectionStateToVisibilityConverter. (C) 2020 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class ConnectionStateToVisibilityConverter : IValueConverter
    {
        #region Properties

        /// <summary>
        ///     Value für Status Connected
        /// </summary>
        public bool ConnectedValue { get; set; } = true;

        /// <summary>
        ///     Value für Status Disconnected
        /// </summary>
        public bool DisconnectedValue { get; set; } = false;

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
            if (value is EnumDcConnectionState dcCon)
            {
                switch (dcCon)
                {
                    case EnumDcConnectionState.Connected:
                        return ConnectedValue;
                    case EnumDcConnectionState.Disconnected:
                        return DisconnectedValue;
                }
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