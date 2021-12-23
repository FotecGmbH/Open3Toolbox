// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using System.Text.RegularExpressions;

namespace ExConfigExchange.Annotations
{
    /// <summary>
    /// String Fields/Properties die mit diesem Attribute gekennzeichnet wurden, werden mit hilfe der <see cref="RegexAttribute.RegexPattern"/> validiert.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class RegexAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegexAttribute"/> class.
        /// </summary>
        /// <param name="regexPattern">The regex pattern.</param>
        public RegexAttribute(string regexPattern)
        {
            Regex.IsMatch("", regexPattern);

            RegexPattern = regexPattern;
        }

        /// <summary>
        /// Der Regex Muster.
        /// </summary>
        public string RegexPattern { get; }
    }
}
