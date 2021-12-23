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

using System;
using System.Threading.Tasks;
using ExchangeLibrary.DataBaseData.Interfaces.Models;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using ExchangeLibrary.ExchangeData;

namespace ExchangeLibrary.DataBaseData.Entities
{
    /// <summary>
    ///     Represents the data, measured by  measurement.
    /// </summary>
    /// <seealso cref="Interfaces.Models.IEntity" />
    /// <seealso cref="Interfaces.Validation.IValidationVisitable" />
    public class TableMeasurementData : MeasurementValue, IEntity, IValidationVisitable
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public long Id { get; set; }

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     Accepts the specified validator.
        /// </summary>
        /// <param name="validator">The validator to be used to validate this instance.</param>
        public void Accept(IValidationVisitor validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            validator.Validate(this);
        }

        /// <summary>
        ///     Accepts the specified asynchronous validator.
        /// </summary>
        /// <param name="validator">The asynchronous validator to be used to validate this instance.</param>
        /// <returns>A task for async operation.</returns>
        public Task AcceptAsync(IValidationVisitor validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            return validator.ValidateAsync(this);
        }

        #endregion
    }
}