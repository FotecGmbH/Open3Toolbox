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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Biss.AppConfiguration;
using Biss.Apps.Base.Connectivity.Model;
using Biss.Apps.Service.Push;
using Biss.Dc.Core;
using Biss.Dc.Transport.Server.SignalR;
using Biss.Log.Producer;
using Database.Context;
using Database.Tables;
using Exchange;
using Exchange.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebExchange;
using WebServer.DataConnector;
using WebServer.Helper;
using WebServer.Models;
using WebServer.Services;

namespace WebServer
{
    /// <summary>
    ///     The startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Welche Swagger Version soll verwendet werden (v1, v2, v3)
        /// </summary>
        private const string SwaggerVersion = "v2";

        /// <summary>
        ///     Initializes a new instance
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Properties

        /// <summary>
        ///     The configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        ///     For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        /// </summary>
        /// <param name="services">The services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("WebAppSettings");
            services.Configure<WebAppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<WebAppSettings>();

            // configure jwt authentication
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                                                  {
                                                      ValidateIssuerSigningKey = true,
                                                      ValidateLifetime = true,
                                                      IssuerSigningKey = new SymmetricSecurityKey(key),
                                                      ValidateIssuer = false,
                                                      ValidateAudience = false,
                                                      ClockSkew = TimeSpan.Zero
                                                  };
                });

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    if (Constants.AppConfiguration.CurrentBuildType != EnumCurrentBuildType.CustomerRelease)
                    {
                        builder.AllowAnyOrigin();
                    }
                    else
                    {
                        //builder.AllowAnyOrigin();
                        builder.WithOrigins(" https://open3toolboxwebserverrelease.azurewebsites.net", AppSettings.Current().DcSignalHost);
                        builder.WithOrigins("https://open3toolboxadminapprelease.azurewebsites.net", AppSettings.Current().DcSignalHost);
                        //TODO: Bei Release hier die richtiten URLs einstellen
                    }

                    builder.AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddScoped<LoginModel>();

            //Für EMail Versand
            services.AddScoped<ViewRenderer, ViewRenderer>();
            services.AddScoped<EMailService, EMailService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().AddControllersAsServices();


            #region Initialize Swagger

            if (Constants.AppConfiguration.CurrentBuildType != EnumCurrentBuildType.CustomerRelease)
            {
                string additionalInformation = "";

                if (Constants.AppConfiguration.CurrentBuildType == EnumCurrentBuildType.Developer)
                {
                    additionalInformation = "<br><br>";
                    additionalInformation += "Für geschützte API Zugriffe muss ein Token mitangegeben werden.<br>";
                    additionalInformation += "Schritt 1: ";
                    additionalInformation += @"Token anfordern: Abschnitt Users -> /api/authenticate <br>";
                    additionalInformation += "Schritt 2: ";
                    additionalInformation += "Rechts oben auf Authorize klicken, diesen Token angeben und auf Authorize klicken.<br>";
                    additionalInformation += "Schritt 3: Die gewünschte Funktion aufrufen.<br>";
                    additionalInformation += "Hinweis: Der Token hat standardmäßig eine Gültigkeit von 30min<br>";
                }
            }

            #endregion

            #region UpdateService

            var db = new DatabaseContext(WebConstants.ConnectionString);

            TableSetting? setting = null;

            try
            {
                setting = db.TblSettings.FirstOrDefault();
            }
            catch (Exception e)
            {
                Logging.Log.LogError($"Settigns load error: {e}");
            }

            #endregion

            #region Health

            #endregion

            ExSaveDataResult.LanguageContent = Language.GetText;

            //services.AddSignalR();

            var signalR = services.AddDcSignalRCore<IServerRemoteCalls>(new ServerRemoteCalls());

            signalR.AddHubOptions((HubOptions<DcCoreHub<IServerRemoteCalls>> opt) => { opt.MaximumReceiveMessageSize = 1000000000; });

            services.AddTelerikBlazor();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] {"application/octet-stream"});
            });

            // Push Initialisieren
            PushService.Initialize(WebSettings.Current());
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Die app</param>
        /// <param name="env">Die environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_host");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });


            app.UseEndpoints(endpoints => { endpoints.MapHub<DcCoreHub<IServerRemoteCalls>>(DcHelper.DefaultHubRoute); });

            Logging.Init(c => c.AddDebug().SetMinimumLevel(LogLevel.Trace).AddConsole().SetMinimumLevel(LogLevel.Trace));
            Logging.Log.LogTrace("[ServerApp] Launch App ...");
        }

        /// <summary>
        ///     Schreiben einer Antwort
        /// </summary>
        /// <param name="context">Der Kontext</param>
        /// <param name="result">Das Ergebnis</param>
        /// <returns>Ein Task</returns>
        private static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            var options = new JsonWriterOptions
                          {
                              Indented = true
                          };

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartObject();
                    writer.WriteString("status", result.Status.ToString());
                    writer.WriteStartObject("results");
                    foreach (var entry in result.Entries)
                    {
                        writer.WriteStartObject(entry.Key);
                        writer.WriteString("status", entry.Value.Status.ToString());
                        writer.WriteString("description", entry.Value.Description);
                        writer.WriteString("exception", entry.Value.Exception?.ToString() ?? string.Empty);
                        writer.WriteStartObject("data");
                        foreach (var item in entry.Value.Data)
                        {
                            writer.WritePropertyName(item.Key);
                            JsonSerializer.Serialize(
                                writer, item.Value, item.Value?.GetType() ??
                                                    typeof(object));
                        }

                        writer.WriteEndObject();
                        writer.WriteEndObject();
                    }

                    writer.WriteEndObject();
                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                return context.Response.WriteAsync(json);
            }
        }

        /// <summary>
        ///     Infos über ein Assembly sammeln
        /// </summary>
        /// <param name="assembly">Assembly (zB Assembly.GetExecutingAssembly() für den Code der gerade läuft)</param>
        /// <returns>Ein Ping ergebnis</returns>
        private ExPingResult GetAssemblyInfos(Assembly assembly)
        {
            var assemblyDate = DateTime.MinValue;
            string assemblyVersion = "?";
            if (assembly != null)
            {
                var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
                if (attribute != null)
                {
                    assemblyVersion = attribute.InformationalVersion;
                }

                try
                {
                    assemblyDate = File.GetLastWriteTime(assembly.Location);
                }
                catch
                {
                    Logging.Log.LogWarning("Cannot read assembly Date.");
                }
            }

            var info = new ExPingResult {VersionNr = assemblyVersion, VersionUpdatedAt = assemblyDate};
            return info;
        }
    }
}