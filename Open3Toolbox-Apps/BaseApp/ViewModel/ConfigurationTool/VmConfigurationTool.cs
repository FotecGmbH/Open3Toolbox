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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.ViewModel.ConfigurationTool.Interfaces;
using Biss.Apps.Interfaces;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Biss.Interfaces;
using Biss.Serialize;
using Exchange.Model.ConfigurationTool;
using Exchange.Resources.ResConfigurationTool;
using Exchange.Services.ConfigurationTool;
using ExchangeLibrary.ConfigInterfaces;
using ExchangeLibrary.Helper;
using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace BaseApp.ViewModel.ConfigurationTool
{
    /// <summary>
    ///     ViewModel vom Konfigurationstool.
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    public class VmConfigurationTool : VmProjectBase
    {
        /// <summary>
        ///     Feuert wenn einen Projekt/Sensor/Gateway entfernt wurde, sodass der View sich aktualisieren kann.
        /// </summary>
        public EventHandler OnIExConfigurableRemoved = null!;

        /// <summary>
        ///     Der ausgwählten Wert.
        /// </summary>
        private object selectedValue = null!;

        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        public VmConfigurationTool() : base(ResViewConfigurationTool.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Der Ausgewählte Wert aus dem Baum.
        /// </summary>
        public object SelectedValue
        {
            get => selectedValue;
            set
            {
                if (selectedValue != null && selectedValue != value
                                          && selectedValue is IVmEditable editable)
                {
                    try
                    {
                        editable.UndoChanges();
                    }
                    catch (ObjectDisposedException)
                    {
                        /// Can occure if the user recklessly Navigates around, but it is unimportant.
                    }
                }

                try
                {
                    selectedValue = value;
                }
                catch (ObjectDisposedException)
                {
                    // Muss nicht behandelt werden.
                }
            }
        }

        /// <summary>
        ///     Die Projekte vom Benutzer.
        /// </summary>
        public ICollection<VmProject> Projects { get; } = new List<VmProject>();

        /// <summary>
        ///     Neues Projekt zufügen.
        /// </summary>
        public VmCommand CmdAddProject { get; set; } = null!;

        /// <summary>
        ///     Neues Gateway zufügen.
        /// </summary>
        public VmCommand CmdAddGateway { get; set; } = null!;

        /// <summary>
        ///     Neues Sensor zufügen.
        /// </summary>
        public VmCommand CmdAddSensor { get; set; } = null!;

        /// <summary>
        ///     Projekt/Sensor/Gateway entfernen.
        ///     <remark>Ich weiß es ist nicht schön.</remark>
        /// </summary>
        public VmCommand CmdRemoveIExConfigurable { get; set; } = null!;

        /// <summary>
        ///     Die Gateways von dem ausgewählten Projekt verbinden.
        /// </summary>
        public VmCommand CmdToGatewayConnectorView { get; set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args"></param>
        public override async Task OnActivated(object? args = null)
        {
            IsBusy = true; // Busy Lock In

            await Dc.DcExProjectTemplates.WaitDataFromServerAsync().ConfigureAwait(true);
            await Dc.DcExGatewayTemplates.WaitDataFromServerAsync().ConfigureAwait(true);
            await Dc.DcExSensorTemplates.WaitDataFromServerAsync().ConfigureAwait(true);
            await Dc.DcExUserProjects.WaitDataFromServerAsync().ConfigureAwait(true);
            await Dc.DcExUserGateways.WaitDataFromServerAsync().ConfigureAwait(true);
            await Dc.DcExUserSensors.WaitDataFromServerAsync().ConfigureAwait(true);
            InitializeProjects();

            IsBusy = false; // Busy Lock Out

            await base.OnActivated(args).ConfigureAwait(true);
        }

        /// <summary>
        ///     View wurde inaktiv
        /// </summary>
        /// <param name="view">Die View</param>
        /// <returns>Task</returns>
        public override Task OnDisappearing(IView view)
        {
            SelectedValue = null!;
            return base.OnDisappearing(view);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdAddProject = new VmCommand(ResViewConfigurationTool.Cmd_AddProject, async () =>
            {
                var toAdd = Dc.DcExProjectTemplates.FirstOrDefault();
                if (toAdd is null)
                {
                    return;
                }

                var dP = new DcListDataPoint<ExProject>(DeepClone(toAdd.Data));
                Dc.DcExUserProjects.Add(dP);
                var result = await Dc.DcExUserProjects.StoreAll().ConfigureAwait(true);
                if (!result.DataOk)
                {
                    Toast.Show(ResViewConfigurationTool.Error, result.ServerExceptionText);
                    return;
                }

                var vmProject = new VmProject(dP);
                Projects.Add(vmProject);
                SelectedValue = vmProject;
                Toast.Show($"{toAdd.Data.Name} {ResViewConfigurationTool.NotificationMsg_ItemAdded}");
            });

            CmdAddGateway = new VmCommand(ResViewConfigurationTool.Cmd_AddGateway, async o =>
            {
                if (o is VmProject projectVm)
                {
                    var toAdd = Dc.DcExGatewayTemplates.FirstOrDefault();
                    if (toAdd is null)
                    {
                        return;
                    }

                    toAdd.Data.ProjectId = projectVm.DataPoint.Index;
                    var dP = new DcListDataPoint<ExGateway>(DeepClone(toAdd.Data));
                    Dc.DcExUserGateways.Add(dP);
                    var result = await Dc.DcExUserGateways.StoreAll().ConfigureAwait(true);
                    if (!result.DataOk)
                    {
                        Toast.Show(ResViewConfigurationTool.Error, result.ServerExceptionText);
                        return;
                    }

                    var vmGateway = new VmGateway(dP, projectVm.RemoveGatewayVm);
                    projectVm.AddGatewayVm(vmGateway);
                    SelectedValue = vmGateway;
                    Toast.Show($"{toAdd.Data.Name} {ResViewConfigurationTool.NotificationMsg_ItemAdded}");
                }
            });

            CmdAddSensor = new VmCommand(ResViewConfigurationTool.Cmd_AddSensor, async o =>
            {
                if (o is VmGateway gatewayVm)
                {
                    var toAdd = Dc.DcExSensorTemplates.FirstOrDefault();
                    if (toAdd is null)
                    {
                        return;
                    }

                    toAdd.Data.GatewayId = gatewayVm.DataPoint.Index;
                    var dP = new DcListDataPoint<ExSensor>(DeepClone(toAdd.Data));
                    Dc.DcExUserSensors.Add(dP);
                    var result = await Dc.DcExUserSensors.StoreAll().ConfigureAwait(true);


                    var vmSensor = new VmSensor(dP, gatewayVm.RemoveSensorVm);
                    gatewayVm.AddSensorVm(vmSensor);
                    SelectedValue = vmSensor;
                    Toast.Show($"{toAdd.Data.Name} {ResViewConfigurationTool.NotificationMsg_ItemAdded}");
                }
            });

            CmdRemoveIExConfigurable = new VmCommand("", async () =>
            {
                if (SelectedValue is VmProject projectVm)
                {
                    Dc.DcExUserProjects.Remove(projectVm.DataPoint);
                    var result = await Dc.DcExUserProjects.StoreAll().ConfigureAwait(true);
                    if (!result.DataOk)
                    {
                        Toast.Show(ResViewConfigurationTool.Error, result.ServerExceptionText);
                        return;
                    }

                    Toast.Show($"{projectVm.DataPoint.Data.Name} {ResViewConfigurationTool.NotificationMsg_ItemDeleted}");
                    Projects.Remove(projectVm);
                    OnIExConfigurableRemoved?.Invoke(this, new EventArgs());
                }
                else if (SelectedValue is VmGateway gatewayVm)
                {
                    Dc.DcExUserGateways.Remove(gatewayVm.DataPoint);
                    var result = await Dc.DcExUserGateways.StoreAll().ConfigureAwait(true);
                    if (!result.DataOk)
                    {
                        Toast.Show(ResViewConfigurationTool.Error, result.ServerExceptionText);
                        return;
                    }

                    Toast.Show($"{gatewayVm.DataPoint.Data.Name} {ResViewConfigurationTool.NotificationMsg_ItemDeleted}");
                    gatewayVm.Dispose();
                    OnIExConfigurableRemoved?.Invoke(this, new EventArgs());
                }
                else if (SelectedValue is VmSensor sensorVm)
                {
                    Dc.DcExUserSensors.Remove(sensorVm.DataPoint);
                    var result = await Dc.DcExUserSensors.StoreAll().ConfigureAwait(true);
                    if (!result.DataOk)
                    {
                        Toast.Show(ResViewConfigurationTool.Error, result.ServerExceptionText);
                        return;
                    }

                    Toast.Show($"{sensorVm.DataPoint.Data.Name} {ResViewConfigurationTool.NotificationMsg_ItemDeleted}");
                    sensorVm.Dispose();
                    OnIExConfigurableRemoved?.Invoke(this, new EventArgs());
                }
                else
                {
                    throw new NotImplementedException("Type Handling not implemented!");
                }

                SelectedValue = null!;
            });

            CmdToGatewayConnectorView = new VmCommand($"{ResViewConnectorCommon.Cmd_Next} {ResViewConfigurationTool.VisitGatewayConnector}", () =>
            {
                if (SelectedValue is VmProject project)
                {
                    try
                    {
                        project.UndoChanges();
                    }
                    catch (NullReferenceException)
                    {
                        /// This can be thrown by <see cref="DcListDataPoint{T}.EndEdit(bool)"/>
                    }

                    var opCodeChips = ChipDllsHandler.GetOpCodeChips(Directory.GetCurrentDirectory() + "\\bin\\Debug\\net5.0\\OpCodeDlls");

                    foreach (var gateway in project.Gateways)
                    foreach (var sensor in gateway.Sensors)
                    {
                        var sens = BissDeserialize.FromJson<Sensor>(ExConfigurableJsonConverter.ToJSON(0, sensor.DataPoint.Data));

                        SensorOpcodesHelper.AddOpCodesTo(sens);
                    }


                    Nav.ToView("ViewGatewayConnector", project);
                }
            }, () => SelectedValue is VmProject);

            base.InitializeCommands();
        }

        /// <summary>
        ///     <see cref="Projects" /> Initializieren.
        /// </summary>
        private void InitializeProjects()
        {
            Projects.Clear();
            foreach (var pD in Dc.DcExUserProjects)
            {
                var vmProject = new VmProject(pD);
                foreach (var gD in Dc.DcExUserGateways.Where(gD => gD.Data.ProjectId == pD.Index))
                {
                    var vmGateway = new VmGateway(gD, vmProject.RemoveGatewayVm);
                    foreach (var sD in Dc.DcExUserSensors.Where(sD => sD.Data.GatewayId == gD.Index))
                    {
                        vmGateway.AddSensorVm(new VmSensor(sD, vmGateway.RemoveSensorVm));
                    }

                    vmProject.AddGatewayVm(vmGateway);
                }

                Projects.Add(vmProject);
            }
        }

        /// <summary>
        ///     Tief Klonen.
        /// </summary>
        /// <typeparam name="T">Der <see cref="Type" />.</typeparam>
        /// <param name="item">Das item.</param>
        /// <returns>
        ///     Einen Tiefen Klone vom
        ///     <param name="item" />
        ///     .
        /// </returns>
        private T DeepClone<T>(T item) where T : IBissSerialize =>
            BissDeserialize.FromJson<T>(item.ToJson());
    }
}