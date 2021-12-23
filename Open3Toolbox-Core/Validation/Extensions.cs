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

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Validation
{
    using System;

    /// <summary>
    ///     Contains Extensionmethods for Validation.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///     Gets added or updated entries form the EF-Core ChangeTracker.
        /// </summary>
        /// <param name="changeTracker">The change tracker.</param>
        /// <returns>The added or updated entries of the ChangeTracker.</returns>
        public static IEnumerable<EntityEntry> AddedOrUpdatedEntries(this ChangeTracker changeTracker)
        {
            return changeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
        }

        /// <summary>
        ///     Checks with reflection if the Type implements the given interface-type.
        /// </summary>
        /// <param name="implementing">The Type to check if the interface is implemented or not.</param>
        /// <param name="implemented">The Type to check if it is implemented.</param>
        /// <returns>Whether "implemented" is implemented in "implementing".</returns>
        public static bool ImplementsInterface(this Type implementing, Type implemented)
        {
            return implementing.GetInterfaces().Contains(implemented);
        }
    }
}