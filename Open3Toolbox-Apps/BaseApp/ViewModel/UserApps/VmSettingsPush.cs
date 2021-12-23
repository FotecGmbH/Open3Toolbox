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
using System.ComponentModel;
using System.Threading.Tasks;
using Biss.Apps.Base;
using Biss.Apps.Interfaces;
using Biss.Apps.Push;
using Biss.Apps.ViewModel;
using Biss.Dc.Core;
using Exchange.Resources;
using Xamarin.Essentials;

// ReSharper disable once CheckNamespace
namespace BaseApp.ViewModel
{
    /// <summary>
    ///     <para>Viewmodel für die Push Settings</para>
    ///     Klasse VmSettingsPush. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmSettingsPush : VmProjectBase
    {
        /// <summary>
        ///     Design Instanz für XAML d:DataContext="{x:Static viewmodels:VmSettingsPush.DesignInstance}"
        /// </summary>
        public static VmSettingsPush DesignInstance = new VmSettingsPush();

        /// <summary>
        ///     Ob view aktiv ist
        /// </summary>
        private bool _viewIsActive;

        /// <summary>
        ///     VmSettingsPush
        /// </summary>
        public VmSettingsPush() : base(ResViewSettingsPush.Title)
        {
            IsBusy = true;
            Push.PushStateChanged += Push_PushStateChanged;
        }

        #region Properties

        /// <summary>
        ///     Command für das öffnen der Push Einstellungen
        /// </summary>
        public VmCommand? CmdOpenDevicePushSettings { get; private set; }

        /// <summary>
        ///     Sind Pushes aktiv
        /// </summary>
        public bool PushEnabled { get; set; }

        /// <summary>
        ///     Ob speichern aktiv ist
        /// </summary>
        public bool SaveActive { get; set; } = true;

        #endregion

        /// <summary>
        ///     Wird aufgerufen sobald die View initialisiert wurde
        /// </summary>
        /// <param name="args">Die Argumente</param>
        /// <returns>Einen Task</returns>
        public override Task OnActivated(object? args = null)
        {
            _viewIsActive = true;
            PushEnabled = Push.CheckPushEnabled();
            SaveActive = Dc.ConnectionState == EnumDcConnectionState.Connected;
            Dc.ConnectionChanged += ConnectionChanged;
            Dc.DcExUserSettings.Update();
            Dc.DcExUserSettings.Data.PropertyChanged += UserDataChanged;
            IsBusy = false;
            return base.OnActivated(args);
        }

        /// <summary>
        ///     View wurde wieder aktiv unbedingt beim überschreiben auch base. aufrufen!
        /// </summary>
        /// <returns></returns>
        public override Task OnAppearing(IView view)
        {
            _viewIsActive = true;
            PushEnabled = Push.CheckPushEnabled();
            return base.OnAppearing(view);
        }

        /// <summary>View wurde inaktiv</summary>
        /// <returns>Einen Task</returns>
        public override Task OnDisappearing(IView view)
        {
            _viewIsActive = false;
            return base.OnDisappearing(view);
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Ob disposing oder nicht</param>
        protected override void Dispose(bool disposing)
        {
            _viewIsActive = false;
            Push.PushStateChanged -= Push_PushStateChanged;
            Dc.ConnectionChanged -= ConnectionChanged;
            Dc.DcExUserSettings.Data.PropertyChanged -= UserDataChanged;
            base.Dispose(disposing);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdOpenDevicePushSettings = new VmCommand(ResViewSettingsPush.Cmd_PushSettings, () =>
            {
                if (XamarinDeviceInfo.SupportsEssentialsAppInfo)
                {
                    AppInfo.ShowSettingsUI();
                    PushEnabled = Push.CheckPushEnabled();
                }
                else
                {
                    MsgBox!.Show(ResViewSettingsPush.NotSupported);
                }
            });
        }

        /// <summary>
        ///     Event wenn push status Änderung
        /// </summary>
        /// <param name="sender">Der Sender</param>
        /// <param name="e">Die Envent Argumente</param>
        private void Push_PushStateChanged(object sender, PushStateChangedEventArgs e)
        {
            PushEnabled = e.PushEnabled;
        }

        /// <summary>
        ///     Event wenn Verbindungs Änderung
        /// </summary>
        /// <param name="sender">Der Sender</param>
        /// <param name="e">Die Event Argumente</param>
        private void ConnectionChanged(object sender, EnumDcConnectionState e)
        {
            SaveActive = e == EnumDcConnectionState.Connected;
        }

        /// <summary>
        ///     Event wenn benutzerdaten Änderung
        /// </summary>
        /// <param name="sender">Der Sender</param>
        /// <param name="e">Die Event Argumente</param>
        private async void UserDataChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!SaveActive)
            {
                return;
            }

            if (!_viewIsActive)
            {
                return;
            }

            var result = await Dc.DcExUserSettings.StoreData().ConfigureAwait(true);

            if (!result.DataOk)
            {
                await MsgBox!.Show(ResViewSettings.MsgBox_TextSaveError, ResViewSettings.MsgBox_CaptionSaveError, icon: VmMessageBoxImage.Error)
                    .ConfigureAwait(true);
            }
        }
    }
}