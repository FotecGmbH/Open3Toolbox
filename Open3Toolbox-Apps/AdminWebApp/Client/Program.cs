// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       AdminWebApp.Client
// 
// Released under MIT

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Biss.Apps.Blazor.Helper;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;

namespace AdminWebApp.Client
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
        /// <returns>A task</returns>
        public static async Task Main(string[] args)
        {
            Client.Main.Init();

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
            builder.Services.AddScoped(s => new NavArgsHelper());
            builder.Services.AddTelerikBlazor();

            // Radzen
            builder.Services.AddScoped<DialogService>();
            builder.Services.AddScoped<NotificationService>();

            await builder.Build().RunAsync();
        }
    }
}