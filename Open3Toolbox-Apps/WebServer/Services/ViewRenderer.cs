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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace WebServer.Services
{
    /// <summary>
    ///     Renderer für Mails
    /// </summary>
    public class ViewRenderer
    {
        /// <summary>
        ///     Der Service-Anbieter
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        ///     Der Daten-Anbieter
        /// </summary>
        private readonly ITempDataProvider _tempDataProvider;

        /// <summary>
        ///     Die View-Engine
        /// </summary>
        private readonly IRazorViewEngine _viewEngine;

        /// <summary>
        ///     Konstruktor
        /// </summary>
        /// <param name="viewEngine">Die View-Engine</param>
        /// <param name="tempDataProvider">Der Daten-Anbieter</param>
        /// <param name="serviceProvider">Der Service-Anbieter</param>
        public ViewRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        ///     Renderer
        /// </summary>
        /// <typeparam name="TModel">Der Typ des Modells</typeparam>
        /// <param name="name">Der Name</param>
        /// <param name="model">Das Modell</param>
        /// <returns>Der Renderer</returns>
        public string Render<TModel>(string name, TModel model)
        {
            var actionContext = GetActionContext();
            ViewEngineResult viewEngineResult;

            try
            {
                viewEngineResult = _viewEngine.FindView(actionContext, name, false);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
            }

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        new EmptyModelMetadataProvider(),
                        new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                view.RenderAsync(viewContext).GetAwaiter().GetResult();

                return output.ToString();
            }
        }

        /// <summary>
        ///     Action Kontext abfragen
        /// </summary>
        /// <returns></returns>
        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = _serviceProvider;
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}