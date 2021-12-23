// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Istvan Galfi
// Project:       Validation
// 
// Released under MIT

using System.Threading.Tasks;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Validation
{
    using System;

    /// <summary>
    ///     This class performs the validation on the <see cref="IEntity" />.
    /// </summary>
    /// <seealso cref="Interfaces.Validation.IValidationVisitor" />
    public class ValidationVisitor : IValidationVisitor
    {
        /// <summary>
        ///     The service provider, necessary for getting the required validator.
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationVisitor" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <exception cref="ArgumentNullException">serviceProvider</exception>
        public ValidationVisitor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        #region Interface Implementations

        /// <summary>
        ///     Validates the specified entity.
        ///     If no validator is available, the method returns.
        /// </summary>
        /// <typeparam name="T">The type of the entity to be validated.</typeparam>
        /// <param name="entity">The entity to validate.</param>
        /// <seealso cref="IEntity" />
        /// <seealso cref="IValidationVisitable" />
        public void Validate<T>(T entity) where T : IValidationVisitable
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator == null)
            {
                return;
            }

            validator.ValidateAndThrow(entity);
        }

        /// <summary>
        ///     Validates the specified entity asynchronously.
        ///     If no validator is available, the method returns.
        /// </summary>
        /// <typeparam name="T">The type of the entity to be validated.</typeparam>
        /// <param name="entity">The entity to be validated.</param>
        /// <seealso cref="IEntity" />
        /// <seealso cref="IValidationVisitable" />
        public async Task ValidateAsync<T>(T entity) where T : IValidationVisitable
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator == null)
            {
                return;
            }

            await validator.ValidateAndThrowAsync(entity);
        }

        #endregion
    }
}