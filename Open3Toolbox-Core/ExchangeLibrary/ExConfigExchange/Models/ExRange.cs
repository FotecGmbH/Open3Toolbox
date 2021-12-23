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
using Biss.Interfaces;
using ExConfigExchange.Models.Interfaces;

namespace ExConfigExchange.Models
{
    /// <summary>
    ///     Hilfsklasse für die Validierung vom numerischen <see cref="IExConfigItem" /> implementationen.
    /// </summary>
    public class ExRange : IBissSerialize
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExRange" /> class.
        /// </summary>
        /// <param name="min">Der Minimum.</param>
        /// <param name="max">Der Maximum.</param>
        /// <param name="step">Der Schritt mit dem der Wert erhöht/gesenkt werden darf.</param>
        /// <exception cref="System.ArgumentException">
        ///     Ungültigen <see cref="Min" /> und <see cref="Max" /> oder <see cref="Step" />.
        /// </exception>
        public ExRange(double min, double max, double step)
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
        public double Min { get; set; }

        /// <summary>
        ///     Der Maximum validen Wert(Inklusive).
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        ///     Der Schritt mit dem der Wert erhöht/gesenkt werden darf.
        /// </summary>
        public double Step { get; set; }

        #endregion
    }
}