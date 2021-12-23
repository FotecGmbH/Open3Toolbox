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
    ///     The database configuration for <see cref="TablePublishedSensor" />.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.IEntityTypeConfiguration{Exchange.Entities.TablePublishedSensor}" />
    public class TablePublishedSensorConfiguration : IEntityTypeConfiguration<TablePublishedSensor>
    {
        #region Interface Implementations

        /// <summary>
        ///     Configers the properties of the entity
        /// </summary>
        /// <param name="builder">The builder for the specified entity</param>
        public void Configure(EntityTypeBuilder<TablePublishedSensor> builder)
        {
            builder.HasKey(x => new {x.GatewayId, x.Id});
            builder.HasIndex(x => x.Name).IsUnique();
            builder.HasOne(x => x.Gateway)
                .WithMany(x => x.Sensors)
                .HasForeignKey(x => new {x.ProjectId, x.GatewayId});
        }

        #endregion
    }
}