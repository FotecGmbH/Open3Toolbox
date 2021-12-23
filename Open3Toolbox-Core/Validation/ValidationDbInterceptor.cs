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

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Validation
{
    /// <summary>
    ///     This Database-Interceptor checks every added or updated entry for validity, according to the available Validator
    ///     for the specific entry.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.Diagnostics.SaveChangesInterceptor" />
    public class ValidationDbInterceptor : SaveChangesInterceptor
    {
        /// Modified by Istvan:
        /// <see cref="IServiceProvider" />
        /// to
        /// <see cref="IValidationVisitor" />
        private readonly IValidationVisitor _validationVisitor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationDbInterceptor" /> class.
        /// </summary>
        public ValidationDbInterceptor(IValidationVisitor validationVisitor)
        {
            _validationVisitor = validationVisitor ?? throw new ArgumentNullException(nameof(validationVisitor));
        }

        /// <summary>
        ///     Called at the start of <see cref="M:DbContext.SaveChanges" />.
        ///     Checks before saving to the database if all added and updated entries are valid.
        ///     Throws an <see cref="ValidationException" /> if an entry is invalid.
        /// </summary>
        /// <param name="eventData">
        ///     Contextual information about the <see cref="Microsoft.EntityFrameworkCore.DbContext" /> being
        ///     used.
        /// </param>
        /// <param name="result">
        ///     Represents the current result if one exists.
        ///     This value will have <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> set
        ///     to <see langword="true" /> if some previous
        ///     interceptor suppressed execution by calling
        ///     <see cref="M:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.SuppressWithResult(`0)" />.
        ///     This value is typically used as the return value for the implementation of this method.
        /// </param>
        /// <returns>
        ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is false, the EF will
        ///     continue as normal.
        ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is true, then EF will
        ///     suppress the operation it
        ///     was about to perform and use <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.Result" />
        ///     instead.
        ///     A normal implementation of this method for any interceptor that is not attempting to change the result
        ///     is to return the <paramref name="result" /> value passed in.
        /// </returns>
        /// <exception cref="ValidationException">Thrown, when an entry is no valid.</exception>
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var entries = eventData.Context.ChangeTracker.AddedOrUpdatedEntries().Where(x => x.Metadata.ClrType.ImplementsInterface(typeof(IValidationVisitable)));

            foreach (var entry in entries)
            {
                var entity = entry.Entity as IValidationVisitable;
                entity.Accept(_validationVisitor);
            }

            return result;
        }

        /// <summary>
        ///     Called at the start of <see cref="M:DbContext.SaveChangesAsync" />.
        ///     Checks before saving to the database if all added and updated entries are valid.
        ///     Throws an <see cref="ValidationException" /> if an entry is invalid.
        /// </summary>
        /// <param name="eventData">
        ///     Contextual information about the <see cref="Microsoft.EntityFrameworkCore.DbContext" /> being
        ///     used.
        /// </param>
        /// <param name="result">
        ///     Represents the current result if one exists.
        ///     This value will have <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> set
        ///     to <see langword="true" /> if some previous
        ///     interceptor suppressed execution by calling
        ///     <see cref="M:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.SuppressWithResult(`0)" />.
        ///     This value is typically used as the return value for the implementation of this method.
        /// </param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is false, the EF will
        ///     continue as normal.
        ///     If <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.HasResult" /> is true, then EF will
        ///     suppress the operation it
        ///     was about to perform and use <see cref="P:Microsoft.EntityFrameworkCore.Diagnostics.InterceptionResult`1.Result" />
        ///     instead.
        ///     A normal implementation of this method for any interceptor that is not attempting to change the result
        ///     is to return the <paramref name="result" /> value passed in.
        /// </returns>
        /// <exception cref="ValidationException">Thrown, when an entry is no valid.</exception>
        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var entries = eventData.Context.ChangeTracker.AddedOrUpdatedEntries().Where(x => x.Metadata.ClrType.ImplementsInterface(typeof(IValidationVisitable)));
            var tasks = entries.Select(entry =>
            {
                var entity = entry.Entity as IValidationVisitable;
                return entity.AcceptAsync(_validationVisitor);
            });

            await Task.WhenAll(tasks);
            return result;
        }
    }
}