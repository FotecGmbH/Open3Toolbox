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
using System.Threading.Tasks;
using ExchangeLibrary.DataBaseData.Entities.Bases;
using ExchangeLibrary.DataBaseData.Interfaces.Models;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;

namespace ExchangeLibrary.DataBaseData.Entities
{
    using System;

    /// <summary>
    ///     Represents a gateway, which contains <see cref="TablePublishedSensor" />s.
    /// </summary>
    /// <seealso cref="Interfaces.Models.IEntity" />
    /// <seealso cref="Interfaces.Validation.IValidationVisitable" />
    public class TablePublishedGateway : TableBaseGateway, IEntity, IValidationVisitable
    {
        #region Properties

        /// <summary>
        ///     The SetupId is used to identify the gateway in the configuration-file.
        ///     It is used during the first registration of the Gateway on the server.
        ///     After the registration the gateway gets the <see cref="Id" />, which is used for further actions.
        /// </summary>
        public Guid SetupId { get; set; } = Guid.NewGuid();

        /// <summary>
        ///     The project this gateway is part of.
        /// </summary>
        public TablePublishedProject Project { get; set; }

        /// <summary>
        ///     The sensors, which are part of this gateway.
        /// </summary>
        public ICollection<TablePublishedSensor> Sensors { get; set; }

        #endregion

        /// <summary>
        ///     Returns a representative <see cref="String" /> of this instance.
        /// </summary>
        /// <returns>Returns the name of the gateway.</returns>
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