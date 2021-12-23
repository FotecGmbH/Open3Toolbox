// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       02.07.2021 10:19
// Developer:     Istvan Galfi
// Project:       Database
// 
// Released under MIT

using System.Reflection;
using Database.Tables;
using Database.Tables.ConfigurationTool;
using Database.Tables.Statistics;
using ExchangeLibrary.DataBaseData.Entities;
using Microsoft.EntityFrameworkCore;
using Validation;

namespace Database.Context
{
    /// <summary>
    ///     Represents the EF-Core DatabaseContext for this application.
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class DatabaseContext : DbContext
    {
        /// <summary>
        ///     The validation database interceptor.
        /// </summary>
        private readonly ValidationDbInterceptor _validationDbInterceptor = null!;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DatabaseContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="validationDbInterceptor">The validation database interceptor.</param>
        public DatabaseContext(DbContextOptions<DatabaseContext> options, ValidationDbInterceptor validationDbInterceptor) : base(options)
        {
            _validationDbInterceptor = validationDbInterceptor;
        }

        public DatabaseContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        #region Properties

        /// <summary>
        ///     Connectionstring
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        ///     Tabelle User
        /// </summary>
        public virtual DbSet<TableUser> TblUsers { get; set; }

        /// <summary>
        ///     Tabelle Settings
        /// </summary>
        public virtual DbSet<TableSetting> TblSettings { get; set; }

        /// <summary>
        ///     Tabelle Devices
        /// </summary>
        public virtual DbSet<TableDevice> TblDevices { get; set; }

        /// <summary>
        ///     ViewStates für Statistik
        /// </summary>
        public virtual DbSet<TableViewState> TblViewStates { get; set; }

        /// <summary>
        ///     Tabelle Files z.B. Userbild
        /// </summary>
        public virtual DbSet<TableFile> TblFiles { get; set; }

        /// <summary>
        ///     Gets the companies.
        /// </summary>
        /// <value>
        ///     The companies.
        /// </value>
        public virtual DbSet<TableUserCompany> TblUserCompanies { get; private set; }

        /// <summary>
        ///     Gets the gateways.
        /// </summary>
        /// <value>
        ///     The gateways.
        /// </value>
        public virtual DbSet<TableUserGateway> TblUserGateways { get; private set; }

        /// <summary>
        ///     Gets the projects.
        /// </summary>
        /// <value>
        ///     The projects.
        /// </value>
        public virtual DbSet<TableUserProject> TblUserProjects { get; private set; }

        /// <summary>
        ///     Gets the sensors.
        /// </summary>
        /// <value>
        ///     The sensors.
        /// </value>
        public virtual DbSet<TableUserSensor> TblUserSensors { get; private set; }

        /// <summary>
        ///     Gets the companies.
        /// </summary>
        /// <value>
        ///     The companies.
        /// </value>
        public virtual DbSet<TablePublishedCompany> TblPublishedCompanies { get; private set; }

        /// <summary>
        ///     Gets the gateways.
        /// </summary>
        /// <value>
        ///     The gateways.
        /// </value>
        public virtual DbSet<TablePublishedGateway> TblPublishedGateways { get; private set; }

        /// <summary>
        ///     Gets the projects.
        /// </summary>
        /// <value>
        ///     The projects.
        /// </value>
        public virtual DbSet<TablePublishedProject> TblPublishedProjects { get; private set; }

        /// <summary>
        ///     Gets the sensors.
        /// </summary>
        /// <value>
        ///     The sensors.
        /// </value>
        public virtual DbSet<TablePublishedSensor> TblPublishedSensors { get; private set; }

        /// <summary>
        ///     Gets the measurement data.
        /// </summary>
        /// <value>
        ///     The measurement data.
        /// </value>
        public virtual DbSet<TableMeasurementData> TblMeasurementData { get; private set; }

        /// <summary>
        ///     Veröffentlichte Aktoren => Optionen Beim Aktor auswahl
        /// </summary>
        public virtual DbSet<TablePublishedActor> TblPublishedActors { get; set; }

        /// <summary>
        ///     Veröffentlichte Messungen => Optionen Beim Messung auswahl
        /// </summary>
        public virtual DbSet<TablePublishedMeasurement> TblPublishedMeasurements { get; set; }

        /// <summary>
        ///     Tabelle für den SubViews vom Benutzer. (z.B.s.: Haus) (Statistik Teil von Konfigurationstool)
        /// </summary>
        public virtual DbSet<TableSubView> TblSubViews { get; set; }

        /// <summary>
        ///     Tabelle für den FinalSubViews vom Benutzer. (z.B.s.: Badezimmer) (Statistik Teil von Konfigurationstool)
        /// </summary>
        public virtual DbSet<TableFinalSubView> TblFinalSubViews { get; set; }

        /// <summary>
        ///     Tabelle für den ActorView vom Benutzer. (z.B.s.: Lichtschalter) (Statistik Teil von Konfigurationstool)
        /// </summary>
        public virtual DbSet<TableActorView> TblActorViews { get; set; }

        /// <summary>
        ///     Tabelle für den MeasurementView vom Benutzer. (z.B.s.: Temperatursensor) (Statistik Teil von Konfigurationstool)
        /// </summary>
        public virtual DbSet<TableMeasurementView> TblMeasurementViews { get; set; }

        #endregion

        /// <summary>
        ///     Db Context initialisieren - für SQL Server
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_validationDbInterceptor != null)
            {
                optionsBuilder.AddInterceptors(_validationDbInterceptor);
            }

            optionsBuilder.EnableDetailedErrors();

            if (!string.IsNullOrWhiteSpace(ConnectionString))
            {
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        /// <summary>
        ///     Override this method to further configure the model that was discovered by convention from the entity types
        ///     exposed in <see cref="Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting
        ///     model may be cached
        ///     and re-used for subsequent instances of your derived context.
        ///     Used to apply the database configurations for the existing entities.
        /// </summary>
        /// <param name="modelBuilder">
        ///     The builder being used to construct the model for this context. Databases (and other extensions) typically
        ///     define extension methods on this object that allow you to configure aspects of the model that are specific
        ///     to a given database.
        /// </param>
        /// <remarks>
        ///     If a model is explicitly set on the options for this context (via
        ///     <see
        ///         cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />
        ///     )
        ///     then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}