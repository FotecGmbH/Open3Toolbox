// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Istvan Galfi
// Project:       Database
// 
// Released under MIT

using System;
using Database.Context;

namespace Database.Extensions
{
    /// <summary>
    ///     Contains extenions methods for the <see cref="Database.Context.DatabaseContext" />.
    /// </summary>
    public static class ContextExtensions
    {
        /// <summary>
        ///     Ensures that the db is created
        /// </summary>
        /// <param name="context">The database context to check</param>
        public static void Initialize(this DatabaseContext context)
        {
            if (!context.Database.EnsureCreated())
            {
                throw new OperationCanceledException("db could not be created");
            }
        }
    }
}