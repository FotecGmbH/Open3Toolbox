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
using System.Text.RegularExpressions;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    ///     String Fields/Properties die mit diesem Attribute gekennzeichnet wurden, werden mit hilfe der
    ///     <see cref="RegexAttribute.RegexPattern" /> validiert.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RegexAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegexAttribute" /> class.
        /// </summary>
        /// <param name="regexPattern">The regex pattern.</param>
        public RegexAttribute(string regexPattern)
        {
            Regex.IsMatch("", regexPattern);

            RegexPattern = regexPattern;
        }

        #region Properties

        /// <summary>
        ///     Der Regex Muster.
        /// </summary>
        public string RegexPattern { get; }

        #endregion
    }
}