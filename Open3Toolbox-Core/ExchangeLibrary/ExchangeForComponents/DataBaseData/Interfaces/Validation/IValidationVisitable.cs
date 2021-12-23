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

using System.Threading.Tasks;

namespace ExchangeLibrary.DataBaseData.Interfaces.Validation
{
    /// <summary>
    ///     That an IValidationVisitor can visit the class which implements this interface.
    /// </summary>
    public interface IValidationVisitable
    {
        /// <summary>
        ///     Accepts a visitor for validation
        /// </summary>
        /// <param name="validator">validation visitor</param>
        void Accept(IValidationVisitor validator);

        /// <summary>
        ///     Accepts a visitor for validation async
        /// </summary>
        /// <param name="validator">validation visitor</param>
        /// <returns>A task</returns>
        Task AcceptAsync(IValidationVisitor validator);
    }
}