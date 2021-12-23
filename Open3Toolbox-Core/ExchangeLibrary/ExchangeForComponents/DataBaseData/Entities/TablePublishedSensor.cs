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
using ExchangeLibrary.DataBaseData.Entities.Bases;
using ExchangeLibrary.DataBaseData.Interfaces.Models;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;

namespace ExchangeLibrary.DataBaseData.Entities
{
    /// <summary>
    ///     Represents the sensor, which is part of a gateway.
    /// </summary>
    /// <seealso cref="Interfaces.Models.IEntity" />
    /// <seealso cref="Interfaces.Validation.IValidationVisitable" />
    public class TablePublishedSensor : TableBaseSensor, IEntity, IValidationVisitable
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the gateway.
        /// </summary>
        /// <value>
        ///     The gateway, which contains this sensor.
        /// </value>
        public TablePublishedGateway Gateway { get; set; }

        /// <summary>
        ///     The id of the project this sensor is part of.
        /// </summary>
        public long ProjectId { get; set; }

        #endregion

        /// <summary>
        ///     Returns the name of the sensor.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        ///     The name of the sensor.
        /// </returns>
        public override string ToString() => Name;

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