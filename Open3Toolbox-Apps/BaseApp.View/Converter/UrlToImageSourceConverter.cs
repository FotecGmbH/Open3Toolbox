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
using System.IO;
using Exchange.Resources;
using Xamarin.Forms;

namespace BaseApp.Converter
{
    /// <summary>
    ///     <para>Url to ImageSource converter.</para>
    ///     Klasse UrlToImageSourceConverter. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class UrlToImageSourceConverter : IValueConverter
    {
        #region Interface Implementations

        /// <summary>
        ///     Konvertiert ein Objekt für XAML
        /// </summary>
        /// <param name="value">Wert zum konvertieren für das UI</param>
        /// <param name="targetType">Zieltyp des Werts</param>
        /// <param name="parameter">
        ///     Parameter to define the fallback image. In this case its "user" for the user picture or "group"
        ///     for the group picture.
        /// </param>
        /// <param name="culture">Aktuelle Kultur</param>
        /// <returns>Konvertierter Wert oder null</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null!)
            {
                if (parameter is string p && p != null! && !string.IsNullOrWhiteSpace(p))
                {
                    if (p.ToUpperInvariant().Equals("MSTEAMS"))
                    {
                        return ImageSource.FromStream(() => Images.ReadImageAsStream(EnumEmbeddedImage.MsTeams_png));
                    }

                    if (p.ToUpperInvariant().Equals("MSTODO"))
                    {
                        return ImageSource.FromStream(() => Images.ReadImageAsStream(EnumEmbeddedImage.MsTodo_png));
                    }
                }

                if (value is Stream stream)
                {
                    return ImageSource.FromStream(() => stream);
                }

                if (value is string url)
                {
                    if (!string.IsNullOrEmpty(url))
                    {
#pragma warning disable CA1310 // Specify StringComparison for correctness
                        if (url.StartsWith("http"))
#pragma warning restore CA1310 // Specify StringComparison for correctness
                        {
                            return ImageSource.FromUri(new Uri(url));
                        }

                        if (File.Exists(url))
                        {
                            return ImageSource.FromFile(url);
                        }
                    }

                    if (parameter is string param && param != null! && !string.IsNullOrWhiteSpace(param))
                    {
                        if (param.ToUpperInvariant().Equals("USER"))
                        {
                            return ImageSource.FromStream(() => Images.ReadImageAsStream(EnumEmbeddedImage.DefaultUserImage_png));
                        }
                    }
                }
            }

            return ImageSource.FromStream(() => Images.ReadImageAsStream(EnumEmbeddedImage.Logo_png));
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
            return null!;
        }

        #endregion
    }
}