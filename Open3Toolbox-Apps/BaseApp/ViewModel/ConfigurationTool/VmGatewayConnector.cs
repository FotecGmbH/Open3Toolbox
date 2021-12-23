// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       30.08.2021 15:55
// Developer:     Istvan Galfi
// Project:       BaseApp
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Biss.Serialize;
using Exchange.Model.ConfigurationTool;
using Exchange.Resources.ResConfigurationTool;
using Exchange.Services.ConfigurationTool;
using ExchangeLibrary.Helper;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;
using ExConfigExchange.Models;

namespace BaseApp.ViewModel.ConfigurationTool
{
    /// <summary>
    ///     ViewModel vom GatewayConnector.
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    public class VmGatewayConnector : VmProjectBase
    {
        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        public VmGatewayConnector() : base(ResViewGatewayConnector.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Die noch nicht verbundene Gateways.
        /// </summary>
        public ICollection<VmGateway> FreeGateways { get; private set; } = new List<VmGateway>();

        /// <summary>
        ///     Die Noch nicht verbundene UUIDs.
        /// </summary>
        public ICollection<string> FreeUUIDs { get; private set; } = new List<string>();

        /// <summary>
        ///     Der ausgewählte UUID.
        /// </summary>
        public string SelectedUUID { get; set; } = null!;

        /// <summary>
        ///     Der ausgewählte Gateway.
        /// </summary>
        public VmGateway SelectedGateway { get; set; } = null!;

        /// <summary>
        ///     Der Projekt.
        /// </summary>
        public VmProject Project { get; private set; } = new VmProject();

        /// <summary>
        ///     UUIDs Aktualisiren.
        /// </summary>
        public VmCommand CmdRefreshUUIDs { get; private set; } = null!;

        /// <summary>
        ///     Ausgewählten Gateway mit ausgewälten UUID verbinden.
        /// </summary>
        public VmCommand CmdConnectGateway { get; private set; } = null!;

        /// <summary>
        ///     Command für das herunterladen der gateway settings
        /// </summary>
        public VmCommand CmdDownloadGatewaySettings { get; private set; } = null!;

        /// <summary>
        ///     Weiter zum Sensor verbindung Seite.
        /// </summary>
        public VmCommand CmdToSensorConnectorView { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args"></param>
        public async override Task OnActivated(object? args = null)
        {
            if (args is VmProject p)
            {
                FreeGateways = p.Gateways.ToList(); // .ToList creates a shallow copy.
                Project = p;
            }
            else
            {
                Nav.ToView("ViewConfigurationTool");
            }

            IsBusy = true;
            await RefreshUUIDs().ConfigureAwait(true);
            IsBusy = false;

            await base.OnActivated(args).ConfigureAwait(true);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdRefreshUUIDs = new VmCommand(ResViewConnectorCommon.Cmd_Refresh, async () =>
            {
                IsBusy = true;
                await RefreshUUIDs().ConfigureAwait(true);
                IsBusy = false;
            });

            CmdDownloadGatewaySettings = new VmCommand("download Gateway Settings", async () =>
            {
                var gatewayToDownload = new Gateway
                                        {
                                            Sensors = new List<Sensor>(), Name = SelectedGateway.Name,
                                            Description = SelectedGateway.DataPoint.Data.Description,
                                            ServerUrl = ((ExUrlConfigItem) (SelectedGateway.DataPoint.Data.Configuration["serverUrl"])).Value,
                                            Interval = ((ExIntConfigItem) (SelectedGateway.DataPoint.Data.Configuration["interval"])).Value,
                                            ComToSens = (Comunication) (((ExEnumConfigItem) (SelectedGateway.DataPoint.Data.Configuration["comToSens"])).Selected.Value)
                                        };

                foreach (var sensor in SelectedGateway.Sensors)
                {
                    var sens = BissDeserialize.FromJson<Sensor>(ExConfigurableJsonConverter.ToJSON(0, sensor.DataPoint.Data));
                    SensorOpcodesHelper.AddOpCodesTo(sens);

                    gatewayToDownload.Sensors.Add(sens);
                }


                var jsonString = gatewayToDownload.ToJson();

                string path = Directory.GetCurrentDirectory() + @"\..\..\gateConfigs.json";
                File.WriteAllText(path, jsonString);
            }, () => SelectedGateway != null);

            CmdConnectGateway = new VmCommand(ResViewConnectorCommon.Cmd_Connect, async () =>
            {
                IsBusy = true;
                Dc.DcExPairs.Add(new DcListDataPoint<ExPair>(new ExPair(new KeyValuePair<string, long>(SelectedUUID, SelectedGateway.DataPoint.Index))));

                var result = await Dc.DcExPairs.StoreAll().ConfigureAwait(true);
                Dc.DcExPairs.Clear();
                IsBusy = false;

                if (!result.DataOk)
                {
                    await MsgBox.Show(result.ServerExceptionText).ConfigureAwait(true);
                }
                else
                {
                    FreeGateways.Remove(SelectedGateway);
                    await RefreshUUIDs().ConfigureAwait(true);
                    await MsgBox.Show($"{SelectedGateway.Name} {ResViewConnectorCommon.Msg_Success} {ResViewConnectorCommon.Msg_Connected}").ConfigureAwait(true);
                    SelectedUUID = null!;
                    SelectedGateway = null!;
                }
            }, () => SelectedGateway != null);

            CmdToSensorConnectorView = new VmCommand("Projekt veröffentlichen", () => { Nav.ToView("ViewProjectPublisher", Project); }, () => true);

            base.InitializeCommands();
        }

        /// <summary>
        ///     UUIDs aktualisieren.
        /// </summary>
        private async Task RefreshUUIDs()
        {
            await Dc.DcExUids.WaitDataFromServerAsync().ConfigureAwait(true);
            FreeUUIDs = Dc.DcExUids.Select(d => d.Data.Uid).ToList();
        }
    }
}