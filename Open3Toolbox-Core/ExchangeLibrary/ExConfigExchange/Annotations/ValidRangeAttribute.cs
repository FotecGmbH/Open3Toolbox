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
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    ///     Setzt den (z.B.s.) <see cref="ExIntConfigItem.ValidRange" /> Property von numerische <see cref="IExConfigItem" />
    ///     implementationen für die Validierung.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ValidRangeAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidRangeAttribute" /> class.
        /// </summary>
        /// <param name="min">Der Minimum.</param>
        /// <param name="max">Der Maximum.</param>
        /// <param name="step">Der Schritt mit dem der Wert erhöht/gesenkt werden darf.</param>
        /// <exception cref="System.ArgumentException">
        ///     Ungültigen <see cref="Min" /> und <see cref="Max" /> oder <see cref="Step" />.
        /// </exception>
        public ValidRangeAttribute(double min, double max, double step)
        {
            if (min > max)
            {
                throw new ArgumentException($"{nameof(min)} cannot be greater than {nameof(max)}.");
            }

            if (step < 0)
            {
                throw new ArgumentException($"{nameof(step)} cannot be negative.");
            }

            Min = min;
            Max = max;
            Step = step;
        }

        #region Properties

        /// <summary>
        ///     Der Minimum validen Wert(Inklusive).
        /// </summary>
        public double Min { get; }

        /// <summary>
        ///     Der Maximum validen Wert(Inklusive).
        /// </summary>
        public double Max { get; }

        /// <summary>
        ///     Der Schritt mit dem der Wert erhöht/gesenkt werden darf.
        /// </summary>
        public double Step { get; }

        #endregion
    }
}