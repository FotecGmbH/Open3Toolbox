// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       WebServer
// 
// Released under MIT

using System;
using System.IO;
using ExchangeLibrary.ConfigInterfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebServer
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
            // necessary, because, here he get the opcodechips

            var x = ChipDllsHandler.GetOpCodeChips(Directory.GetCurrentDirectory() + "\\bin\\Debug\\net5.0\\OpCodeDlls");

            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///     Creates the host builder
        /// </summary>
        /// <param name="args">The arguments</param>
        /// <returns>The host builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}