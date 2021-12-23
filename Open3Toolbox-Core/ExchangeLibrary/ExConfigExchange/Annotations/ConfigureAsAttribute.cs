// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:50
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    ///     Fields/Properties die mit diesem Attribute gekennzeichnet wurden, werden als
    ///     <see cref="ConfigureAsAttribute.Target" /> von dem benutzer konfiguriert können, aber bei serializierung in dem
    ///     Serializierungsformat abgespeichert. (z.Bs: Json in string.)
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConfigureAsAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigureAsAttribute" /> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException">target</exception>
        public ConfigureAsAttribute(Type target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (!(target.IsInterface || target.IsAbstract || (target.IsClass && target != typeof(Delegate))))
            {
                throw new ArgumentException($"{nameof(target)} can only be a class, interface or an abstract class.");
            }

            Target = target ?? throw new ArgumentNullException(nameof(target));
        }

        #region Properties

        /// <summary>
        ///     Der Type, der bei Kofigurieren verwendet wird.
        /// </summary>
        public Type Target { get; }

        #endregion
    }
}