// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       30.08.2021 08:25
// Developer:     Istvan Galfi
// Project:       Database
// 
// Released under MIT

using ExchangeLibrary.DataBaseData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.ContextConfigurations
{
    /// <summary>
    ///     The database configuration for <see cref="TablePublishedProject" />.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Exchange.Entities.TablePublishedProject}" />
    public class TablePublishedProjectConfiguration : IEntityTypeConfiguration<TablePublishedProject>
    {
        #region Interface Implementations

        /// <summary>
        ///     Configers the properties of the entity
        /// </summary>
        /// <param name="builder">The builder for the specified entity</param>
        public void Configure(EntityTypeBuilder<TablePublishedProject> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Name).IsUnique();
            builder.Property(x => x.Id).ValueGeneratedNever(); // Published Projects sollten die Selbe Id habe wie dessen User Project Gegenteil
            builder.HasMany(x => x.Gateways).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId);
        }

        #endregion
    }
}