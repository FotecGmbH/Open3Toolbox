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
    ///     The database configuration for <see cref="TableUserProject" />.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Exchange.Entities.TableUserProject}" />
    public class TableUserProjectConfiguration : IEntityTypeConfiguration<TableUserProject>
    {
        #region Interface Implementations

        /// <summary>
        ///     Configers the properties of the entity
        /// </summary>
        /// <param name="builder">The builder for the specified entity</param>
        public void Configure(EntityTypeBuilder<TableUserProject> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Gateways).WithOne(x => x.Project).HasForeignKey(x => x.ProjectId);
        }

        #endregion
    }
}