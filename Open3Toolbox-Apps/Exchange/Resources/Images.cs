// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System;
using System.IO;
using System.Reflection;

// ReSharper disable InconsistentNaming
namespace Exchange.Resources
{
    /// <summary>
    ///     Bilder in den Resourcen
    /// </summary>
    public enum EnumEmbeddedImage
    {
        Logo_png,
        Logo2_png,
        Logo3_png,
        DefaultUserImage_png,
        DefaultUserImageSmall_png,
        appbarcard1_png,
        appbarcard2_png,
        appbarcard3_png,
        Pin_png,
        SplashScreenHorizontal_png,
        SplashScreenVertical_png,
        BissBackground_png,
        fotecVert_png,
        ClusteredPin_png,
        MsTeams_png,
        MsTodo_png,
    }

    /// <summary>
    ///     <para>Bilder laden (Projektweit)</para>
    ///     Klasse Images. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public static class Images
    {
        /// <summary>
        ///     Bild als lokalen Stream Laden
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public static Stream ReadImageAsStream(EnumEmbeddedImage imageName)
        {
#pragma warning disable CA1307 // Specify StringComparison for clarity
            var image = $"Exchange.Resources.Images.{imageName.ToString().Replace("_", ".")}";
#pragma warning restore CA1307 // Specify StringComparison for clarity
            Assembly _assembly = Assembly.Load(new AssemblyName("Exchange"));
            Stream _imageStream = _assembly.GetManifestResourceStream(image);
            return _imageStream;
        }
    }
}