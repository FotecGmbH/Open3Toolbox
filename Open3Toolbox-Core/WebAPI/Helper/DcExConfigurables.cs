// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Licensed under MIT

using Biss.Dc.Core;
using Biss.Log.Producer;
using Biss.Serialize;
using Database.Context;
using Database.Tables.ConfigurationTool;
using Database.Tables.Statistics;
using Exchange.Model.ConfigurationTool;
using Exchange.Services.ConfigurationTool;
using ExchangeLibrary.ConfigInterfaces;
using ExchangeLibrary.DataBaseData.Entities;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ExchangeLibrary.Helper;
using ExConfigExchange;
using OpCodesDllsLibrary;
using WebExchange;
using DeviceHelper = Exchange.Helper.DeviceHelper;
using ExConfigExchange.Services;

namespace WebServer
{
    /// <summary>
    /// <para>Datenaustausch für die Veröffentlichung von Projekten.</para>
    /// Klasse ServerRemoteCalls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    /// <seealso cref="WebServer.DataConnector.ServerRemoteCallBase" />
    /// <seealso cref="WebServer.DataConnector.IServerRemoteCalls" />
    public partial class ServerRemoteCalls
    {

        private static ExConfigItemManager _configItemManager = new ExConfigItemManager(new LocalObjectExConfigItemTemplateCacher());
        private static ExConfigurableManager _configurableManager = new ExConfigurableManager(_configItemManager, new LocalExConfigurableCacher());
        private static LocalCacher<ExProject> _projectCacher = new LocalCacher<ExProject>();
        private static  LocalCacher<ExGateway> _gatewayCacher = new LocalCacher<ExGateway>();
        private static LocalCacher<ExSensor> _sensorCacher = new LocalCacher<ExSensor>();

        /// <summary>
        /// Device fordert Listen Daten für UserProjects
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>
        /// Daten oder eine Exception auslösen
        /// </returns>
        public async Task<List<DcServerListItem<ExProject>>> GetDcExPublishedProjects(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            var newUserId = userId == -1 ? 1 : userId;
            using var db = new DatabaseContext(WebConstants.ConnectionString);

            // Nur einen Projekt wird immer gettet, aber das ist besser als FirstOrDefault
            var projects = db.TblPublishedProjects.Where(p => p.Id == startIndex) // TODO Benutzer spezifischen Filtering
                                                  .Select(p => MapPublished(p, false)); // Gateways usw werden nicht gemappt.

            /// Dann <see cref="IExConfigurableManager.ConvertProject(Project)"/>
            var toReturn = projects.Select(p => new KeyValuePair<long, ExProject>(p.Id, _configurableManager.ConvertProject(p)));

            return toReturn.Select(p => new DcServerListItem<ExProject>() { Data = p.Value, Index = p.Key, SortIndex = p.Key }).ToList();
        }


        public Project GetPrepProject(long newUserId, DcStoreListItem<ExProject> carrier)
        {
            /// Anmerkung, Ich fusche hier...
            var cachedProject = _projectCacher.GetCachedItems(newUserId).First(kv => kv.Key == carrier.Index).Value;
            var pGateways = _gatewayCacher.GetCachedItems(newUserId).Where(kv => kv.Value.ProjectId == carrier.Index).Select(kv => new Tuple<long, ExGateway>(kv.Key, kv.Value)).ToList();
            var gatewayIds = pGateways.Select(g => g.Item1).ToHashSet();
            var pSensors = _sensorCacher.GetCachedItems(newUserId).Where(kv => gatewayIds.Contains(kv.Value.GatewayId)).Select(kv => new Tuple<long, ExSensor>(kv.Key, kv.Value)).ToList();
            return this.PrepareProject(carrier.Index, cachedProject, pGateways, pSensors);
        }




        /// <summary>
        /// Device will Listen Daten für UserProjects sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>
        /// Ergebnis (bzw. Infos zum Fehler)
        /// </returns>
        public async Task<DcListStoreResult> StoreDcExPublishedProjects(long deviceId, long userId, List<DcStoreListItem<ExProject>> data, long secondId)
        {
            var newUserId = userId == -1 ? 1 : userId;
            var carrier = data.FirstOrDefault();
            if (carrier is null)
                return new DcListStoreResult() { StoreResult = new DcStoreResult() { ErrorType = EnumServerError.None } };
            Project project = GetPrepProject(newUserId, carrier);


            var actorsAndMeasurements = project is null ? null : GetActorsAndMeasurements(project, newUserId);

            var tt = project.ToJson();


            using var db = new DatabaseContext(WebConstants.ConnectionString);
            //db.SavedChanges += Db_SavedChanges;
            project.Company = Map(db.TblUserCompanies.First(c => c.Email == project.Company.Email && c.Name == project.Company.Name)); // The Ids of companies are not set
            try
            {
                switch (carrier.State)
                {
                    case EnumDcListElementState.New:
                        try
                        {
                            // Benutze false im .ConfigureAwait im BackEnd, laut Mike.
                            await SaveActorsAndMeasurementsChanges(db, actorsAndMeasurements, project.Id).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            Logging.Log.LogError($"Store UserProject Error: {e}");
                            return new DcListStoreResult() { StoreResult = new DcStoreResult() { ErrorType = EnumServerError.Connection, ServerExceptionText = e.Message } };
                        }

                        if (db.TblPublishedCompanies.Any(c => c.Id == project.Company.Id))
                            db.TblPublishedCompanies.Update(MapPublished(project.Company, newUserId));
                        else
                            db.TblPublishedCompanies.Add(MapPublished(project.Company, newUserId));

                        db.TblPublishedProjects.Add(MapPublished(project, newUserId));
                        await db.SaveChangesAsync().ConfigureAwait(false);
                        return new DcListStoreResult() { ElementsStored = 1, NewIndex = new List<DcListStoreResultIndex>() { new DcListStoreResultIndex() { BeforeStoreIndex = carrier.Index, NewIndex = carrier.Index } } };

                    case EnumDcListElementState.Modified:
                        try
                        {
                            await SaveActorsAndMeasurementsChanges(db, actorsAndMeasurements, project.Id).ConfigureAwait(false);
                        }
                        catch (Exception e)
                        {
                            var innerExceptionMessage = e.InnerException is null ? "" : $"\nInner Excepion:{e.InnerException.Message}";
                            Logging.Log.LogError($"Store UserProject Error: {e}");
                            return new DcListStoreResult() { StoreResult = new DcStoreResult() { ErrorType = EnumServerError.Connection, ServerExceptionText = $"Outer Exception: {e.Message}{innerExceptionMessage}" } };
                        }

                        var toUpdate = MapPublished(project, newUserId);

                        ////////////////////////////
                        /*var publishedProject = db.TblPublishedProjects.FirstOrDefault(p => p.Id == toUpdate.Id);
                        publishedProject.Company = toUpdate.Company;
                        publishedProject.Gateways = toUpdate.Gateways;
                        publishedProject.CompanyId = toUpdate.CompanyId;
                        publishedProject.Name = toUpdate.Name;
                        publishedProject.Id = toUpdate.Id;*/
                        var entity = db.TblPublishedProjects.Update(toUpdate);
                        ////////////////////////


                        //var entity = db.TblPublishedProjects.Update(toUpdate);
                        await db.SaveChangesAsync().ConfigureAwait(false);
                        return new DcListStoreResult() { ElementsStored = 1, NewIndex = new List<DcListStoreResultIndex>() { new DcListStoreResultIndex() { BeforeStoreIndex = carrier.Index, NewIndex = carrier.Index } } };

                        // Eigentlich Delete Funktion gibt es nicht...
                        //case EnumDcListElementState.Deleted:
                        //    db.TblPublishedProjects.Remove(MapPublished(project, newUserId));
                        //    await db.SaveChangesAsync().ConfigureAwait(false);
                        //    _projectCacher.DeleteCachedItem(userId, carrier.Index);
                        //    return new DcListStoreResult() { ElementsStored = -1 };
                }
            }
            catch (Exception e)
            {
                // Es ist hässlich aber wird benötigt.
                try
                {
                    await RemoveActorsAndMeasurements(db, actorsAndMeasurements).ConfigureAwait(false);
                }
                catch (Exception innerE)
                {
                    return new DcListStoreResult() { StoreResult = new DcStoreResult() { ErrorType = EnumServerError.Connection, ServerExceptionText = $"Outer Exception: {e.Message}\nInner Exception: {innerE.Message}" } };
                }

                Logging.Log.LogError($"Store UserProject Error: {e}");
                var innerExceptionMessage = e.InnerException is null ? "" : $"\nInner Excepion:{e.InnerException.Message}";
                return new DcListStoreResult() { StoreResult = new DcStoreResult() { ErrorType = EnumServerError.Connection, ServerExceptionText = $"Outer Exception: {e.Message}{innerExceptionMessage}" } };
            }

            //db.SavedChanges -= Db_SavedChanges;

            return new DcListStoreResult(); // Just to satisfy Visual Studio
        }

        private static Company Map(TableUserCompany company)
        {
            return new Company()
            {
                Id = company.Id,
                Email = company.Email,
                Name = company.Name,
            };
        }

        /*private void Db_SavedChanges(object? sender, SavedChangesEventArgs e)
        {
            ///////  TODO something changed, let the server know.....or mqtt publish ////////////
            throw new Exception("TODO!");
        }*/

        /// <summary>
        /// Löscht(falls vorhanden) und dann Speichert die Aktoren und Messungen vom Projekt mit projectId.
        /// </summary>
        /// <param name="db">Der <see cref="DbContext"/>.</param>
        /// <param name="actorsAndMeasurements">Die Aktoren und Messungen.</param>
        /// <param name="projectId">Der project identifier.</param>
        /// <returns>Die Anzahl an gespeicherten Aktoren und Messungen.</returns>
        private static async Task SaveActorsAndMeasurementsChanges(DatabaseContext db, Tuple<List<Tuple<TableActorView, TablePublishedActor>>, List<Tuple<TableMeasurementView, TablePublishedMeasurement>>> actorsAndMeasurements, long projectId)
        {
            var actorIds = db.TblPublishedActors.Select(x => x.Id).ToHashSet();
            var actorViewToFinalSubViewMap = db.TblActorViews.ToDictionary(a => a.ActorId, a => a.FinalSubViewId);
            
            var measurementIds = db.TblPublishedMeasurements.Select(x => x.Id).ToHashSet();
            var measurementViewToFinalSubViewMap = db.TblMeasurementViews.ToDictionary(a => a.MeasurementId, a => a.FinalSubViewId);

            // Alle Aktoren und Messungen vom Projekt erst entfernen
            var actorIdsToRemove = db.TblPublishedActors.Where(x => x.ProjectId == projectId).Select(x => x.Id).ToHashSet();
            db.TblActorViews.RemoveRange(db.TblActorViews.Where(x => actorIdsToRemove.Contains(x.ActorId)));
            db.TblPublishedActors.RemoveRange(db.TblPublishedActors.Where(x => actorIdsToRemove.Contains(x.Id)));

            var measurementIdsToRemove = db.TblPublishedMeasurements.Where(x => x.ProjectId == projectId).Select(x => x.Id).ToHashSet();
            db.TblMeasurementViews.RemoveRange(db.TblMeasurementViews.Where(x => measurementIdsToRemove.Contains(x.MeasurementId)));
            db.TblPublishedMeasurements.RemoveRange(db.TblPublishedMeasurements.Where(x => measurementIdsToRemove.Contains(x.Id)));

            foreach (var actorPair in actorsAndMeasurements.Item1)
            {
                var id = actorPair.Item2.Id;
                if (actorIds.Contains(id))
                    actorPair.Item1.FinalSubViewId = actorViewToFinalSubViewMap[id];

                db.TblActorViews.Add(actorPair.Item1);
                db.TblPublishedActors.Add(actorPair.Item2);
            }

            foreach (var measurementPair in actorsAndMeasurements.Item2)
            {
                var id = measurementPair.Item2.Id;
                if (measurementIds.Contains(id))
                    measurementPair.Item1.FinalSubViewId = measurementViewToFinalSubViewMap[id];

                db.TblMeasurementViews.Add(measurementPair.Item1);
                db.TblPublishedMeasurements.Add(measurementPair.Item2);
            }

            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Löscht die Aktoren und Messungen.
        /// </summary>
        /// <param name="db">Der <see cref="DbContext"/>.</param>
        /// <param name="actorsAndMeasurements">Die Aktoren und Messungen.</param>
        /// <returns>Die Anzahl an gespeicherten Aktoren und Messungen.</returns>
        private static async Task RemoveActorsAndMeasurements(DatabaseContext db, Tuple<List<Tuple<TableActorView, TablePublishedActor>>, List<Tuple<TableMeasurementView, TablePublishedMeasurement>>> actorsAndMeasurements)
        {
            foreach (var actorPair in actorsAndMeasurements.Item1)
            {
                db.TblActorViews.Remove(actorPair.Item1);
                db.TblPublishedActors.Remove(actorPair.Item2);
            }

            foreach (var measurementPair in actorsAndMeasurements.Item2)
            {
                db.TblMeasurementViews.Remove(measurementPair.Item1);
                db.TblPublishedMeasurements.Remove(measurementPair.Item2);
            }

            await db.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Holt alle Aktoren und Messungen aus dem Projekt aus.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Alle Aktoren und Messungen des Projekts.</returns>
        /// <exception cref="System.NotImplementedException">Implement Interface Type handling here.</exception>
        private static Tuple<List<Tuple<TableActorView, TablePublishedActor>>, List<Tuple<TableMeasurementView, TablePublishedMeasurement>>> GetActorsAndMeasurements(Project project, long userId)
        {
            // Ich bin kein Fan von so etwas, aber es ist erheblich effizienter als es getrennt durchzuführen.
            var toReturn = new Tuple<List<Tuple<TableActorView, TablePublishedActor>>, List<Tuple<TableMeasurementView, TablePublishedMeasurement>>>(new List<Tuple<TableActorView, TablePublishedActor>>(), new List<Tuple<TableMeasurementView, TablePublishedMeasurement>>());
            foreach (var i in project.Gateways.SelectMany(g => g.Sensors).SelectMany(s => s.Interfaces))
            {
                if (i is I2CInterface i2c)
                {
                    foreach (var chip in i2c.I2cChips)
                    {
                        foreach (var actor in chip.Actors)
                            toReturn.Item1.Add(new Tuple<TableActorView, TablePublishedActor>(MapView(actor, userId), Map(actor, userId, project.Id)));

                        foreach (var measurement in chip.Measurements)
                            toReturn.Item2.Add(new Tuple<TableMeasurementView, TablePublishedMeasurement>(MapView(measurement, userId), Map(measurement, userId, project.Id)));
                    }
                }
                else if (i is GpioInterface gpio)
                {
                    foreach (var chip in gpio.GpioChips)
                    {
                        foreach (var actor in chip.Actors)
                            toReturn.Item1.Add(new Tuple<TableActorView, TablePublishedActor>(MapView(actor, userId), Map(actor, userId, project.Id)));

                        foreach (var measurement in chip.Measurements)
                            toReturn.Item2.Add(new Tuple<TableMeasurementView, TablePublishedMeasurement>(MapView(measurement, userId), Map(measurement, userId, project.Id)));
                    }
                }
                else
                    throw new NotImplementedException("Implement Interface Type handling here.");
            }

            return toReturn;
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="measurement">Der Messung.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns></returns>
        private static TableMeasurementView MapView(Measurement measurement, long userId)
        {
            return new TableMeasurementView()
            {
                MeasurementId = measurement.Id,
                Name = measurement.Name,
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="measurement">Der Messung.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TablePublishedMeasurement Map(Measurement measurement, long userId, long projectId)
        {
            return new TablePublishedMeasurement()
            {
                ProjectId = projectId,
                Name = measurement.Name,
                Description = "To Be Implemented!",
                Id = measurement.Id,
                //Interval = measurement.Interval,
                Port = measurement.Port,
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TableActorView MapView(Actor actor, long userId)
        {
            return new TableActorView()
            {
                ActorId = actor.Id,
                Name = actor.Name,
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TablePublishedActor Map(Actor actor, long userId, long projectId)
        {
            return new TablePublishedActor()
            {
                ProjectId = projectId,
                Name = actor.Name,
                Description = "To Be Implemented!",
                Id = actor.Id,
                Port = actor.Port,
                SetterType = Exchange.Enum.ActorSetterType.Range, // Dummy
                Value = 0, // Dummy
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        /// <param name="deepMap">if set to <c>true</c> [deep map].</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Project MapPublished(TablePublishedProject project, bool deepMap = true)
        {
            return new Project()
            {
                Id = project.Id,
                Company = deepMap ? MapPublished(project.Company) : new Company(),
                Description = project.Description,
                Name = project.Name,
                Gateways = deepMap ? project.Gateways.Select(g => MapPublished(g)).ToList() : new List<Gateway>(),
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Company MapPublished(TablePublishedCompany company)
        {
            return new Company()
            {
                Id = company.Id,
                Email = company.Email,
                Name = company.Name,
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="gateway">The gateway.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Gateway MapPublished(TablePublishedGateway gateway)
        {
            return new Gateway()
            {
                Id = gateway.Id,
                Name = gateway.Name,
                Description = gateway.Description,
                ComToSens = gateway.ComToSens,
                Interval = gateway.Interval,
                ServerUrl = gateway.ServerUrl,
                Sensors = gateway.Sensors.Select(s => MapPublished(s)).ToList(),
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="sensor">The sensor.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static Sensor MapPublished(TablePublishedSensor sensor)
        {
            return new Sensor()
            {
                SensorId = sensor.Id,
                Name = sensor.Name,
                Description = sensor.Description,
                //Type = sensor.Type,
                MeasureInterval = sensor.MeasureInterval,
                MeasureXTimesTillSend = sensor.MeasureXTimesTillSend,
                //SendInterval = sensor.SendInterval,
                Interfaces = BissDeserialize.FromJson<List<CommunicationInterface>>(sensor.JsonStringInfo),
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="project">Der Projekt.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TablePublishedProject MapPublished(Project project, long userId)
        {
            return new TablePublishedProject()
            {
                Id = project.Id,
                CompanyId = project.Company.Id,
                // Company = MapPublished(project.Company, userId), Not allowed
                Description = project.Description,
                Name = project.Name,
                Gateways = project.Gateways.Select(g => MapPublished(g, userId, project.Id)).ToList(),
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TablePublishedCompany MapPublished(Company company, long userId)
        {
            return new TablePublishedCompany()
            {
                Id = company.Id,
                Email = company.Email,
                Name = company.Name,
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="gateway">The gateway.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TablePublishedGateway MapPublished(Gateway gateway, long userId, long projectId)
        {
            return new TablePublishedGateway()
            {
                ProjectId = projectId,
                ComToSens = gateway.ComToSens,
                Id = gateway.Id,
                Name = gateway.Name,
                Description = gateway.Description,
                Interval = gateway.Interval,
                ServerUrl = gateway.ServerUrl,
                Sensors = gateway.Sensors.Select(s => MapPublished(s, userId, gateway.Id, projectId)).ToList(),
            };
        }

        /// <summary>
        /// Mapt die angegebenen <see cref="Type"/>s.
        /// </summary>
        /// <param name="sensor">The sensor.</param>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="gatewayId">The gateway identifier.</param>
        /// <param name="projectId">The project identifier.</param>
        /// <returns>Ein neuer Instanz mit dem angegebenen Werten.</returns>
        private static TablePublishedSensor MapPublished(Sensor sensor, long userId, long gatewayId, long projectId)
        {
            return new TablePublishedSensor()
            {
                GatewayId = gatewayId,
                ProjectId = projectId,
                Id = sensor.SensorId,
                Name = sensor.Name,
                Description = sensor.Description,
                //Type = sensor.Type,
                MeasureInterval = sensor.MeasureInterval,
                MeasureXTimesTillSend = sensor.MeasureXTimesTillSend,
                //SendInterval = sensor.SendInterval,
                JsonStringInfo = sensor.Interfaces.ToJson(),
                AllOpCodes = sensor.AllOpCodes.ToJson()
            };
        }

        /// <summary>
        /// <c>true</c> wenn der MQTT server läuft.
        /// </summary>
        /// Aus Main <see cref="ServerRemoteCalls" />
        public static bool IsRunning = false;

        /// <summary>
        /// Die Geräte, wie Gateways und Sensoren.
        /// </summary>
        private static List<string> Devices = new List<string>();

        /// <summary>
        /// <c>true</c> wenn MQTT initializiert wurde.
        /// </summary>
        private static bool MqttInited;

        /// <summary>
        /// Device fordert Listen Daten für Uids
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>
        /// Daten oder eine Exception auslösen
        /// </returns>
        public async Task<List<DcServerListItem<ExUid>>> GetDcExUids(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            var results = new List<DcServerListItem<ExUid>>();

            results.AddRange(DeviceHelper.DetectPorts().Select(dp => new DcServerListItem<ExUid>(){Data = new ExUid(dp), Index = 1, SortIndex = 1}));

            return results;
            /// Dummy by Istvan
            return new List<DcServerListItem<ExUid>>()
                                   {
                                        new DcServerListItem<ExUid>(){Data = new ExUid(Guid.NewGuid().ToString()), Index = 1, SortIndex = 1},
                                        new DcServerListItem<ExUid>(){Data = new ExUid(Guid.NewGuid().ToString()), Index = 2, SortIndex = 2},
                                        new DcServerListItem<ExUid>(){Data = new ExUid(Guid.NewGuid().ToString()), Index = 3, SortIndex = 3},
                                        new DcServerListItem<ExUid>(){Data = new ExUid(Guid.NewGuid().ToString()), Index = 4, SortIndex = 4},
                                        new DcServerListItem<ExUid>(){Data = new ExUid(Guid.NewGuid().ToString()), Index = 5, SortIndex = 5},
                                    };
            /// Dummy by Istvan

            if (!MqttInited)
                await MqttConfig().ConfigureAwait(true);
            else
                await SendGetUidsAsync().ConfigureAwait(true);

            Thread.Sleep(500);

            var res = new List<DcServerListItem<ExUid>>();

            Devices.ForEach(d => res.Add(new DcServerListItem<ExUid>() { Data = new ExUid(d), Index = 1, SortIndex = 1 }));

            Thread.Sleep(500);

            return res;
        }

        /// <summary>
        /// Device will Listen Daten für Uids sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>
        /// Ergebnis (bzw. Infos zum Fehler)
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<DcListStoreResult> StoreDcExUids(long deviceId, long userId, List<DcStoreListItem<ExUid>> data, long secondId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Device fordert Listen Daten für Pairs (Echte Gateways/Sensoren <-> Logischen Gateways/Sensoren)
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="startIndex">Lesen ab Index (-1 für Start)</param>
        /// <param name="elementsToRead">Anzahl der Elemente welche maximal gelesen werden sollen (-1 für alle verfügbaren Daten)</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <param name="filter">Optionaler Filter für die Daten</param>
        /// <returns>
        /// Daten oder eine Exception auslösen
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<List<DcServerListItem<ExPair>>> GetDcExPairs(long deviceId, long userId, long startIndex, long elementsToRead, long secondId, string filter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Device will Listen Daten für Pairs (Echte Gateways/Sensoren <-> Logischen Gateways/Sensoren) sichern
        /// </summary>
        /// <param name="deviceId">Id des Gerätes</param>
        /// <param name="userId">Id des Benutzers oder -1 wenn nicht angemeldet</param>
        /// <param name="data">Eingetliche Daten</param>
        /// <param name="secondId">Optionale 2te Id um schnellen Wechsel zwischen Listen zu ermöglichen bzw. dynamische Listen. Zb. für Chats</param>
        /// <returns>
        /// Ergebnis (bzw. Infos zum Fehler)
        /// </returns>
        public async Task<DcListStoreResult> StoreDcExPairs(long deviceId, long userId, List<DcStoreListItem<ExPair>> data, long secondId)
        {
            /// Dummy by Istvan
            return new DcListStoreResult();
            /// Dummy by Istvan

            // Your code here
            if (data.All(d => d.State == EnumDcListElementState.Deleted))
                return new DcListStoreResult(); // Task.FromResult(new DcListStoreResult());


            Dictionary<string, string> pairs = new Dictionary<string, string>();
            data.Where(d => d.State == EnumDcListElementState.New).ToList().ForEach(d => pairs.Add(d.Data.Key, d.Data.Value.ToString()));

            await SendPairsAsync(pairs).ConfigureAwait(true);

            return new DcListStoreResult();  // Task.FromResult(new DcListStoreResult());
        }


        /// <summary>
        /// Der MQTT Klient.
        /// </summary>
        private IMqttClient mqttClient;

        /// <summary>
        /// Holt der MQTT Konfiguration.
        /// </summary>
        private async Task MqttConfig()
        {
            mqttClient = (new MqttFactory()).CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("192.168.100.217", 1883)   // hardcoded
                .Build();

            await mqttClient.ConnectAsync(options).ConfigureAwait(true);

            await GetUids().ConfigureAwait(true);
            await SendGetUidsAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// Holt der UIDs.
        /// </summary>
        private async Task GetUids()
        {
            var options = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter(new MqttTopicFilterBuilder().WithTopic("SendUIDs"))
                .Build();
            await mqttClient.SubscribeAsync(options).ConfigureAwait(true);

            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(async e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine("ff: " + e.ApplicationMessage.Topic + " - " + payload);
                Devices = BissDeserialize.FromJson<List<string>>(payload); 
            });
        }

        /// <summary>
        /// Sendet den UIDs.
        /// </summary>
        private async Task SendGetUidsAsync()
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("GetUIDs")
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await mqttClient.PublishAsync(message).ConfigureAwait(true);
        }

        /// <summary>
        /// Sendet die Paaren (Echte Gateways/Sensoren <-> Logischen Gateways/Sensoren).
        /// </summary>
        /// <param name="pairs">The pairs.</param>
        public async Task SendPairsAsync(Dictionary<string, string> pairs)
        {
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("GetPairs")
                .WithPayload(pairs.ToJson())
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();

            await mqttClient.PublishAsync(message).ConfigureAwait(true);
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Konvertiert den Modell Projekt in einem echten Projekt um, und bereitet es für die Veröffentlichung vor.
        /// </summary>
        /// <param name="projectId">The project identifier.</param>
        /// <param name="exProject">The ex project.</param>
        /// <param name="gateways">The gateways.</param>
        /// <param name="sensors">The sensors.</param>
        /// <returns>Die zur Veröffentlichung bereitgestellte Projekt.</returns>
        private Project PrepareProject(long projectId, ExProject exProject, List<Tuple<long, ExGateway>> gateways, List<Tuple<long, ExSensor>> sensors)
        {
            var projectJson = ExConfigurableJsonConverter.ToJSON(projectId, exProject, gateways, sensors);

            var project = BissDeserialize.FromJson<Project>(projectJson);
            
            // Initializiere Commands
            //var z =  ChipDllsHandler.GetOpCodeChips(System.IO.Directory.GetCurrentDirectory() + "\\bin\\Debug\\net5.0\\OpCodeDlls");
            
            //var visitor = new InterfaceConfigVisitor(new List<IConfigChip>());
            foreach (var gateway in project.Gateways)
            foreach (var sensor in gateway.Sensors)
            {
                /*var visitor = new InterfaceOpCodesVisitor( z);

                foreach (var interfaceP in sensor.Interfaces)
                    interfaceP.Accept(visitor);

                sensor.AllOpCodes = visitor.OpCodes;*/

                SensorOpcodesHelper.AddOpCodesTo(sensor);
            }
                    

            return project;
        }

        /// <summary>
        /// Cache-t die Projekte samt Ihrer Gateways, und Sensoren.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="projects">Die Projekte.</param>
        /// <returns>Die Index (<see cref="Project.Id"/>) <-> Modell Projekt (<see cref="ExProject"/>) Paare.</returns>
        private static IEnumerable<KeyValuePair<long, ExProject>> CacheProjectsWithChildren(long userId, IEnumerable<Project> projects)
        {
            foreach (var p in projects)
            {
                var convertedProject = _configurableManager.ConvertProject(p);
                _projectCacher.CacheItem(userId, p.Id, convertedProject);

                _ = CacheGatewaysWithChildren(userId, p.Id, p.Gateways).AsParallel().ToList(); // Using to List here to execute the IEnumerable.

                yield return new KeyValuePair<long, ExProject>(p.Id, convertedProject);
            }
        }

        /// <summary>
        /// Cache-t die Gateways samt Ihrer Sensoren.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="projectId">Der Projekt Id.</param>
        /// <param name="gateways">Die Gateways.</param>
        /// <returns>Die Index (<see cref="Gateway.Id"/>) <-> Modell Gateway (<see cref="ExGateway"/>) Paare.</returns>
        private static IEnumerable<KeyValuePair<long, ExGateway>> CacheGatewaysWithChildren(long userId, long projectId, IEnumerable<Gateway> gateways)
        {
            foreach (var g in gateways)
            {
                var convertedGateway = _configurableManager.ConvertGateway(projectId, g);
                _gatewayCacher.CacheItem(userId, g.Id, convertedGateway);

                _ = CacheSensors(userId, g.Id, g.Sensors).AsParallel().ToList(); // Using to List here to execute the IEnumerable.

                yield return new KeyValuePair<long, ExGateway>(g.Id, convertedGateway);
            }
        }

        /// <summary>
        /// Cache-t die Sensoren.
        /// </summary>
        /// <param name="userId">Der Benutzer Id.</param>
        /// <param name="gatewayId">Der Gateway Id.</param>
        /// <param name="sensors">Die Sensoren.</param>
        /// <returns>Die Index (<see cref="Sensor.Id"/>) <-> Modell Sensor (<see cref="ExSensor"/>) Paare.</returns>
        private static IEnumerable<KeyValuePair<long, ExSensor>> CacheSensors(long userId, long gatewayId, IEnumerable<Sensor> sensors)
        {
            foreach (var s in sensors)
            {
                var convertedSensor = _configurableManager.ConvertSensor(gatewayId, s);
                _sensorCacher.CacheItem(userId, s.SensorId, convertedSensor);
                yield return new KeyValuePair<long, ExSensor>(s.SensorId, convertedSensor);
            }
        }
    }
}
