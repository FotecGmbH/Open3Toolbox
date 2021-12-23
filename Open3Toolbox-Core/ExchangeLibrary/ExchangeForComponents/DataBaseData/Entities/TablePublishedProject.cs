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
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeLibrary.DataBaseData.Entities.Bases;
using ExchangeLibrary.DataBaseData.Interfaces.Models;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;

namespace ExchangeLibrary.DataBaseData.Entities
{
    /// <summary>
    ///     Represents a project, which is the root of a configuration.
    /// </summary>
    /// <seealso cref="Interfaces.Models.IEntity" />
    /// <seealso cref="Interfaces.Validation.IValidationVisitable" />
    public class TablePublishedProject : TableBaseProject, IEntity, IValidationVisitable
    {
        #region Properties

        /// <summary>
        ///     Gets or sets the company.
        /// </summary>
        /// <value>
        ///     The company, which owns this project.
        /// </value>
        public TablePublishedCompany Company { get; set; }

        /// <summary>
        ///     Gets or sets the gateways.
        /// </summary>
        /// <value>
        ///     The gateways, this project contains.
        /// </value>
        public ICollection<TablePublishedGateway> Gateways { get; set; }

        #endregion

        /// <summary>
        ///     Returns the name of the project.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        ///     The name of the project.
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