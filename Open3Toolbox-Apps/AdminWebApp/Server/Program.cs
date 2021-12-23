// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       AdminWebApp.Server
// 
// Released under MIT

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace AdminWebApp.Server
{
    /// <summary>
    ///     The program class
    /// </summary>
    public class Program
    {
        /// <summary>
        ///     The entry point
        /// </summary>
        /// <param name="args">The arguments</param>
        public static void Main(string[] args)
        {
            Client.Main.Init();

            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///     Creates the host builder
        /// </summary>
        /// <param name="args">The arguments</param>
        /// <returns>The hostbuilder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}