// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System.Collections.Generic;
using System.Linq;

namespace ExchangeLibrary.Helper
{
    using System;

    /// <summary>
    ///     Helps to calculate mathematical calculations
    /// </summary>
    public static class CalcHelper
    {
        /// <summary>
        ///     Calculates the greatest common divisor of 2 values
        /// </summary>
        /// <param name="first">First value</param>
        /// <param name="sec">Second value</param>
        /// <returns>The greatest common divisor</returns>
        public static int Ggt(int first, int sec)
        {
            var temp = 0;
            var ggt = 0;
            while (first % sec != 0)
            {
                temp = first % sec;
                first = sec;
                sec = temp;
            }

            ggt = sec;

            return ggt;
        }

        /// <summary>
        ///     Calculates the least common multiple of 2 values
        /// </summary>
        /// <param name="first">First value</param>
        /// <param name="sec">Second value</param>
        /// <returns>The least common multiple</returns>
        public static int KgV(int first, int sec)
        {
            return (first * sec) / Ggt(first, sec);
        }

        /// <summary>
        ///     Calculates the least common multiple of a list of values
        /// </summary>
        /// <param name="numbers">The list of values</param>
        /// <returns>The least common multiple</returns>
        public static int KgVList(List<int> numbers)
        {
            if (numbers == null)
            {
                throw new ArgumentNullException(nameof(numbers));
            }

            if (numbers.Count < 1)
            {
                throw new ArgumentException(nameof(numbers.Count));
            }

            while (numbers.Count > 1)
            {
                var kgVFirstSec = KgV(numbers.ElementAt(0), numbers.ElementAt(1)); // kgV of first and second

                numbers.RemoveRange(0, 2); // remove first and second from list

                numbers.Insert(0, kgVFirstSec); // add the new kgV at the beginning
            }

            return numbers.ElementAt(0);
        }
    }
}