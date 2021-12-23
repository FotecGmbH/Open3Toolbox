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

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Database.Extensions
{
    /// <summary>
    ///     Contains extenions methods for the <see cref="ChangeTracker" />.
    /// </summary>
    public static class ChangeTrackerExtensions
    {
        /// <summary>
        ///     Gets the added or updated entries in the <see cref="ChangeTracker" />.
        /// </summary>
        /// <param name="changeTracker">The change tracker.</param>
        /// <returns>The added or updated entries.</returns>
        public static IEnumerable<EntityEntry> AddedOrUpdatedEntries(this ChangeTracker changeTracker)
        {
            return changeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
        }
    }
}