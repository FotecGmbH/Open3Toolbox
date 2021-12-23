// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Matthias Mandl
// Project:       WebAPI
// 
// Released under MIT

using Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebAPI.Extensions
{
    using System;

    public static class DbInitializationHostExtensions
    {
        public static IHost CreateDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            try
            {
                var dbContext = serviceProvider.GetRequiredService<DatabaseContext>();
                dbContext.Database.SetCommandTimeout(120);
                //dbContext.Database.EnsureDeleted(); // For Reset Purposes
                //dbContext.Initialize();
            }
            catch (Exception e)
            {
                serviceProvider.GetRequiredService<ILogger<Program>>().LogError(e, "An error occured");
            }

            return host;
        }
    }
}