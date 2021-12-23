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

using System.Linq;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;

namespace WebAPI.Extensions
{
    /// <summary>
    ///     Contains exentions-methods for <see cref="Startup" />.
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        ///     Adds the odata formatters to the <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The service-collection to add the odata formatters to.</param>
        public static void AddOdataFormatters(this IServiceCollection services)
        {
            services.AddMvcCore(options =>
            {
                var mediaTypeHeaderValue = "application/prs.odatatestxxx-odata";
                options.OutputFormatters.OfType<ODataOutputFormatter>().Where(formatter => formatter.SupportedMediaTypes.Count == 0).ToList().ForEach(x => x.SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaTypeHeaderValue)));
                options.InputFormatters.OfType<ODataInputFormatter>().Where(formatter => formatter.SupportedMediaTypes.Count == 0).ToList().ForEach(x => x.SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaTypeHeaderValue)));
            });
        }
    }
}