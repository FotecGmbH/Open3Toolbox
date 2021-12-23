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
    ///     Visits entities to validate them
    /// </summary>
    public interface IValidationVisitor
    {
        /// <summary>
        ///     Validates an entity
        /// </summary>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <param name="entity">The entity to validate</param>
        void Validate<T>(T entity) where T : IValidationVisitable;

        /// <summary>
        ///     Validates an entity
        /// </summary>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <param name="entity">The entity to validate</param>
        /// <returns>A task</returns>
        Task ValidateAsync<T>(T entity) where T : IValidationVisitable;
    }
}