// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:50
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.Collections.Generic;
using ExConfigExchange.Models;

namespace ExConfigExchange.Services.Interfaces
{
    /// <summary>
    ///     Cache-t <see cref="ExObjectConfigItem" />s.
    /// </summary>
    public interface IExObjectConfigItemTemplateCacher
    {
        /// <summary>
        ///     Entscheidet ob eine Vorlage für den Objekt vorliegt true oder nicht.
        /// </summary>
        /// <param name="objectType"><see cref="Type.FullName" /> vom Object.</param>
        bool ContainsObjectTemplateFor(string objectType);

        /// <summary>
        ///     Addiert die Vorlage zum Cache.
        /// </summary>
        /// <param name="objectType"><see cref="Type.FullName" /> vom Object.</param>
        /// <param name="template">Die Vorlage.</param>
        void AddObjectTemplateFor(string objectType, ExObjectConfigItem template);

        /// <summary>
        ///     Versucht die Vorlage aus dem Cache zu holen.
        /// </summary>
        /// <param name="objectType"><see cref="Type.FullName" /> vom Object.</param>
        /// <returns>Die Vorlage vom Objekt.</returns>
        ExObjectConfigItem GetObjectTemplateFor(string objectType);

        /// <summary>
        ///     Entscheidet ob Vorlagen(Implementationen) für den Interface vorliegen true oder nicht.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName" /> vom Interface.</param>
        bool ContainsImplementationTemplatesFor(string interfaceType);

        /// <summary>
        ///     Addiert die Vorlagen(Implementationen) zum Cache.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName" /> vom Interface.</param>
        /// <param name="templates">Die Vorlagen.</param>
        void AddImplementationTemplatesFor(string interfaceType, IEnumerable<ExObjectConfigItem> templates);

        /// <summary>
        ///     Addiert die Vorlage(Implementation) zum Cache.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName" /> vom Interface.</param>
        /// <param name="template">Die Vorlage(Implementation).</param>
        void AddImplementationTemplateFor(string interfaceType, ExObjectConfigItem template);

        /// <summary>
        ///     Versucht die Vorlagen(Implementationen) aus dem Cache zu holen.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName" /> vom Interface.</param>
        /// <returns>Die Vorlagen(Implementationen) vom Interface.</returns>
        IEnumerable<ExObjectConfigItem> GetImplementationTemplatesFor(string interfaceType);
    }
}