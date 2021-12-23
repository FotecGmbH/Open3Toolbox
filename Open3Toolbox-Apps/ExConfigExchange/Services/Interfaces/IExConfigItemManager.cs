// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using System.Collections.Generic;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;

namespace ExConfigExchange.Services.Interfaces
{
    /// <summary>
    /// Config Items erlauben das konfigurieren von Modellen ohne dass, der Front-End diese kennen müsste.
    /// </summary>
    public interface IExConfigItemManager
    {
        /// <summary>
        /// Diese Methode Convertiert Existierende Modelle zum <see cref="IExConfigItem"/>s.
        /// Es kann auch dafür eine bereits konfigurierte Modell verwendet werden <param name="defaultValue"/>, in diesem Fall werden dessen werte übernommen.
        /// </summary>
        /// <param name="displayKey">Der display key.</param>
        /// <param name="type">Der zu modellierende type.</param>
        /// <param name="defaultValue">Der default value, aus diesem werden die Werte des <see cref="IExConfigItem"/>s extrahiert falls vorhanden.</param>
        /// <param name="readOnly">Wenn <c>true</c> [read only].</param>
        /// <returns>Einen <see cref="IExConfigItem"/> der den angegebenen <paramref name="type"/> modelliert.</returns>
        IExConfigItem GetIExConfigItemFrom(string displayKey, Type type, object defaultValue = null, bool readOnly = false);

        /// <summary>
        /// Mit dieser Methode kann man die Implementationsvorlage für einen Interface, abstract Klasse oder Klasse der mit der <see cref="ExConfigExchange.Annotations.InterfaceAttribute"/> markiert wurde, abgefragt werden.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName"/> vom <see cref="interface"/>.</param>
        /// <returns>Eine Sammlung von implementationen.</returns>
        IEnumerable<ExObjectConfigItem> GetTemplatesFor(string interfaceType);
    }
}
