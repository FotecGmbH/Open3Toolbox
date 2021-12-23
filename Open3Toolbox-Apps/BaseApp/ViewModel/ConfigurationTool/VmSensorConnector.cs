// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       BaseApp
// 
// Released under MIT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Exchange.Helper;
using Exchange.Model.ConfigurationTool;
using Exchange.Resources.ResConfigurationTool;
using ExConfigExchange.Models;

namespace BaseApp.ViewModel.ConfigurationTool
{
    /// <summary>
    ///     ViewModel vom SensorConnector.
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    public class VmSensorConnector : VmProjectBase
    {
        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        public VmSensorConnector() : base(ResViewSensorConnector.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Die noch nicht verbundene Sensoren.
        /// </summary>
        public ICollection<VmSensor> FreeSensors { get; private set; } = new List<VmSensor>();

        /// <summary>
        ///     Die Noch nicht verbundene UUIDs.
        /// </summary>
        public ICollection<string> FreeUUIDs { get; private set; } = new List<string>();

        /// <summary>
        ///     Der ausgewählte UUID.
        /// </summary>
        public string SelectedUUID { get; set; } = null!;

        /// <summary>
        ///     Der ausgewählte Sensor.
        /// </summary>
        public VmSensor SelectedSensor { get; set; } = null!;

        /// <summary>
        ///     Der Projekt.
        /// </summary>
        public VmProject Project { get; private set; } = new VmProject();

        /// <summary>
        ///     UUIDs aktualisieren.
        /// </summary>
        public VmCommand CmdRefreshUUIDs { get; private set; } = null!;

        /// <summary>
        ///     Ausgewählten Sensor mit ausgewälten UUID verbinden.
        /// </summary>
        public VmCommand CmdConnectSensor { get; private set; } = null!;

        /// <summary>
        ///     Weiter zum Projekt veröffentlichen.
        /// </summary>
        public VmCommand CmdToProjectPublisherView { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args"></param>
        public async override Task OnActivated(object? args = null)
        {
            if (args is VmProject p)
            {
                FreeSensors = p.Gateways.SelectMany(gVM => gVM.Sensors).ToList();
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

            CmdConnectSensor = new VmCommand(ResViewConnectorCommon.Cmd_Connect, async () =>
            {
                IsBusy = true;
                Dc.DcExPairs.Add(new DcListDataPoint<ExPair>(new ExPair(new KeyValuePair<string, long>(SelectedUUID, SelectedSensor.DataPoint.Index))));

                var opCodesConfigItem = SelectedSensor.DataPoint.Data.Configuration["AllOpCodes"];
                var opCodes = ((ExCollectionConfigItem) opCodesConfigItem).Value.Select(exCo => ((ExByteConfigItem) exCo).Value).ToArray();
                DeviceHelper.WriteToSerialPort("test", opCodes);

                var result = await Dc.DcExPairs.StoreAll().ConfigureAwait(true);
                Dc.DcExPairs.Clear();
                IsBusy = false;

                if (!result.DataOk)
                {
                    await MsgBox.Show(result.ServerExceptionText).ConfigureAwait(true);
                }
                else
                {
                    FreeSensors.Remove(SelectedSensor);
                    await RefreshUUIDs().ConfigureAwait(true);
                    await MsgBox.Show($"{SelectedSensor.Name} {ResViewConnectorCommon.Msg_Success} {ResViewConnectorCommon.Msg_Connected}").ConfigureAwait(true);
                    SelectedUUID = null!;
                    SelectedSensor = null!;
                }
            }, () => SelectedSensor != null);

            CmdToProjectPublisherView = new VmCommand($"{ResViewConnectorCommon.Cmd_Next} {ResViewSensorConnector.VisitProjectPublisher}", () => { Nav.ToView("ViewProjectPublisher", Project); }, () => true);

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