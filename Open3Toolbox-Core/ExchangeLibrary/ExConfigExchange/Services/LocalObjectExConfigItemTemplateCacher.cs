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
using System.Collections.Concurrent;
using System.Collections.Generic;
using Biss.Serialize;
using ExConfigExchange.Models;
using ExConfigExchange.Services.Interfaces;

namespace ExConfigExchange.Services
{
    /// <summary>
    /// </summary>
    /// <seealso cref="ExConfigExchange.Services.Interfaces.IExObjectConfigItemTemplateCacher" />
    public class LocalObjectExConfigItemTemplateCacher : IExObjectConfigItemTemplateCacher
    {
        /// <summary>
        ///     Die gechache-te interface Objekt Vorlagen (Implementationen) gemappt zum
        ///     <see cref="Type.FullName" /> vom interface.
        /// </summary>
        private static readonly ConcurrentDictionary<string, ConcurrentBag<ExObjectConfigItem>> _implementations = new ConcurrentDictionary<string, ConcurrentBag<ExObjectConfigItem>>();

        /// <summary>
        ///     Die gechache-te Objekt Vorlagen.
        /// </summary>
        private static readonly ConcurrentDictionary<string, ExObjectConfigItem> _objects = new ConcurrentDictionary<string, ExObjectConfigItem>();

        #region Interface Implementations

        /// <summary>
        ///     Addiert die Vorlage(Implementation) zum Cache.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName" /> vom Interface.</param>
        /// <param name="template">Die Vorlage(Implementation).</param>
        public void AddImplementationTemplateFor(string interfaceType, ExObjectConfigItem template) =>
            _implementations[interfaceType].Add(template);

        /// <summary>
        ///     Addiert die Vorlagen(Implementationen) zum Cache.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName" /> vom Interface.</param>
        /// <param name="templates">Die Vorlagen.</param>
        public void AddImplementationTemplatesFor(string interfaceType, IEnumerable<ExObjectConfigItem> templates) =>
            _implementations.TryAdd(interfaceType, new ConcurrentBag<ExObjectConfigItem>(templates));

        /// <summary>
        ///     Addiert die Vorlage zum Cache.
        /// </summary>
        /// <param name="objectType"><see cref="Type.FullName" /> vom Object.</param>
        /// <param name="template">Die Vorlage.</param>
        public void AddObjectTemplateFor(string objectType, ExObjectConfigItem template) =>
            _objects.TryAdd(objectType, template);

        /// <summary>
        ///     Entscheidet ob Vorlagen(Implementationen) für den Interface vorliegen (<see cref="true" />) oder nicht.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName" /> vom Interface.</param>
        /// <returns></returns>
        public bool ContainsImplementationTemplatesFor(string interfaceType) =>
            _implementations.ContainsKey(interfaceType);

        /// <summary>
        ///     Entscheidet ob eine Vorlage für den Objekt vorliegt (<see cref="true" />) oder nicht.
        /// </summary>
        /// <param name="objectType"><see cref="Type.FullName" /> vom Object.</param>
        /// <returns></returns>
        public bool ContainsObjectTemplateFor(string objectType) =>
            _objects.ContainsKey(objectType);

        /// <summary>
        ///     Versucht die Vorlagen(Implementationen) aus dem Cache zu holen.
        /// </summary>
        /// <param name="interfaceType"><see cref="Type.FullName" /> vom Interface.</param>
        /// <returns>
        ///     Die Vorlagen(Implementationen) vom Interface.
        /// </returns>
        public IEnumerable<ExObjectConfigItem> GetImplementationTemplatesFor(string interfaceType)
        {
            foreach (var implementation in _implementations[interfaceType])
            {
                yield return BissDeserialize.FromJson<ExObjectConfigItem>(implementation.ToJson());
            }
        }

        /// <summary>
        ///     Versucht die Vorlage aus dem Cache zu holen.
        /// </summary>
        /// <param name="objectType"><see cref="Type.FullName" /> vom Object.</param>
        /// <returns>
        ///     Die Vorlage vom Objekt.
        /// </returns>
        public ExObjectConfigItem GetObjectTemplateFor(string objectType) =>
            BissDeserialize.FromJson<ExObjectConfigItem>(_objects[objectType].ToJson());

        #endregion
    }
}