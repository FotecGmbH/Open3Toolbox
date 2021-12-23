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
using System.Globalization;
using System.Reflection;
using System.Resources;
using Exchange.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BaseApp.MarkupExtensions
{
    /// <summary>
    ///     <para>TranslateExtension</para>
    ///     Klasse TranslateExtension. (C) 2018 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    [ContentProperty(nameof(Key))]
    public class TranslateExtension : IMarkupExtension
    {
        /// <summary>
        ///     Resource Name
        /// </summary>
        const string ResourceNamespace = "Exchange.Resources";

        /// <summary>
        ///     Dictionary für string zu ResourceManager
        /// </summary>
        public static Dictionary<string, ResourceManager> Res = new Dictionary<string, ResourceManager>();

        /// <summary>
        ///     Culture Info
        /// </summary>
        readonly CultureInfo _ci;

        /// <summary>
        ///     Initialisiert eine neue Instanz
        /// </summary>
        public TranslateExtension()
        {
            _ci = Language.CurrentCulture;
        }

        #region Properties

        /// <summary>
        ///     Key im Format "ResViewNAME.ID"
        /// </summary>
        public string Key { get; set; }

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     Stellt Wert zur verfügung
        /// </summary>
        /// <param name="serviceProvider">Der ServiceProvider</param>
        /// <returns>Der zur Verfügung gestellte Wert</returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            string translation = String.Empty;

            var tmp = Key.Split('.');
            if (tmp.Length != 2)
            {
                throw new ArgumentException("Language key invalid!");
            }

            string resourceId = $"{ResourceNamespace}.{tmp[0].Trim()}";
            if (!Res.ContainsKey(resourceId))
            {
                ResourceManager res = null;
                try
                {
                    res = new ResourceManager(resourceId, Assembly.GetAssembly(typeof(Language)));
                }
                catch
                {
                    ;
                }

                Res.Add(resourceId, res);
            }

            var resMgr = Res[resourceId];
            if (resMgr == null)
            {
#if DEBUG
                throw new ArgumentException(string.Format($"Resource {resourceId} not found!"));
#else
				translation = Key; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            string key = tmp[1].Trim();

            if (String.IsNullOrEmpty(key))
            {
                return string.Empty;
            }

            translation = resMgr.GetString(key, _ci);
            if (String.IsNullOrEmpty(translation))
            {
#if DEBUG
                throw new ArgumentException(
                    string.Format("Key '{0}' was not found in resources '{1}' for culture '{2}'.", key, resourceId, _ci.Name),
                    "Text");
#else
				translation = Key; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }

            return translation;
        }

        #endregion
    }
}