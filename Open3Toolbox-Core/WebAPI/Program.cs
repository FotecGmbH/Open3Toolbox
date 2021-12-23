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

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebAPI.Extensions;

namespace WebAPI
{
    /// <summary>
    ///     Entry point of the program
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     Entry point of the program
        ///     <param name="args">args</param>
        /// </summary>
        public static async Task Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .CreateDbIfNotExists()
                .Run();
        }

        /// <summary>
        ///     Creates the host builder
        /// </summary>
        /// <param name="args">args</param>
        /// <returns>The Host builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}