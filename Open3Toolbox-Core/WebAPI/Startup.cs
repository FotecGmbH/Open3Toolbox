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
using System.Reflection;
using System.Threading.Tasks;
using Database.Context;
using ExchangeLibrary.DataBaseData.Interfaces.Validation;
using ExchangeLibrary.ExchangeForComponents.MqttCommunication;
using FluentValidation.AspNetCore;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Validation;
using WebAPI.Extensions;
using WebAPI.MqttCommunication;

namespace WebAPI
{
    /// <summary>
    ///     Gets called by the runtime
    ///     Initializes the Project
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Initializes the configuration.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Helper.X();
        }

        #region Properties

        /// <summary>
        ///     Gets the configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        #endregion

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Logging
            services.AddLogging(o => o.AddConsole());

            // Database
            services.AddScoped<IValidationVisitor, ValidationVisitor>();
            services.AddScoped<ValidationDbInterceptor>();
            services.AddDbContext<DatabaseContext>(o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("Database"),
                    sqlServer => { sqlServer.UseNetTopologySuite(); });
            });
            services.AddDatabaseDeveloperPageExceptionFilter();

            // Automapper
            services.AddAutoMapper(Assembly.GetEntryAssembly());

            // Controllers
            // Newtonsoft
            // FluentValidation
            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(ValidationVisitor)));

            // OData
            services.AddOData();

            // Would be for OData v8
            //services.AddOData(o =>
            //{
            //    o.AddModel("odata", GetEdmModel());
            //    //o.EnableCount = true;
            //    //o.EnableExpand = true;
            //    //o.EnableFilter = true;
            //    //o.EnableOrderBy = true;
            //    //o.EnableSelect = true;
            //    //o.EnableSkipToken = true;
            //    //o.MaxTop = 10000;
            //    //o.UrlKeyDelimiter = ODataUrlKeyDelimiter.Slash;
            //});

            // Swagger
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "webapi", Version = "v1"}); });
            // Add Odata Input-/Outputformatters for Swagger
            services.AddOdataFormatters();

            // Mqtt

            services.AddScoped<MqttConnectionHandlerServer>(); // IDs with Mqtt     (if you use this, comment SignalR)


            services.AddScoped<Helper, Helper>();
            services.AddCors(options =>
            {
            options.AddDefaultPolicy(
                builder =>
                {
                    builder.AllowAnyHeader().AllowAnyOrigin();
                });
            });

    // SignalR
    //services.AddSignalR();                                                                                        // IDs with SignalR  (if you use this, comment Mqtt)
    }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder</param>
        /// <param name="env">the web host environment</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MqttConnectionHandlerServer mqttConnectionHandlerServer)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // OData
                endpoints.EnableDependencyInjection();
                endpoints.Select().OrderBy().Filter().Expand().Count().SkipToken().MaxTop(10000);
                //endpoints.MapODataRoute("odata", "odata", GetEdmModel());

                // SignalR
                //endpoints.MapHub<ConfigHub>("/ConfigHub");                                                        // IDs with SignalR     (if you use this, comment Mqtt)
            });

            Task.Run(async () => // if mqtt
            {
                await MqttServer.StartAsync().ConfigureAwait(true);
                await mqttConnectionHandlerServer.StartAsync().ConfigureAwait(true);
            });

            /*Task.Run(async () =>                                           
            {
                while (true)
                {
                    await Task.Delay(500);
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(Configuration.GetConnectionString("Database")))
                        {
                            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM TblPublishedProjects",connection);
                            SqlNotificationRequest sqlNotificationRequest = new SqlNotificationRequest();
                            //sqlNotificationRequest.id="NotificationID";
                            //sqlNotificationRequest.Options..Service="mySSBQueue";
                            sqlNotificationRequest.UserData = "NotificationID";
            

// Associate the notification request with the command.
                            sqlCommand.Notification=sqlNotificationRequest;
// Execute the command.
                            sqlCommand.ExecuteReader();
                        }
                    }
                    catch (Exception e)
                    {
                    }
                    
                }
            });*/
        }
    }
}