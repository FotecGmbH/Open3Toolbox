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
using System.Threading.Tasks;
using Biss.Apps.ViewModel;
using Biss.Dc.Core;
using Exchange;
using Exchange.Resources;
using PropertyChanged;

namespace BaseApp.ViewModel.UserApps
{
    /// <summary>
    ///     <para>Login</para>
    ///     Klasse VmLogin. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmLogin : VmProjectBase
    {
        /// <summary>
        ///     Design Instanz für XAML d:DataContext="{x:Static viewmodels:VmLogin.DesignInstance}"
        /// </summary>
        public static VmLogin DesignInstance = new VmLogin();

        private bool _agbAccepted;

        /// <summary>
        ///     VmLogin
        /// </summary>
        public VmLogin() : base(ResViewLogin.Login)
        {
            IsBusy = true;
        }

        #region Properties

        /// <summary>
        ///     Nutzungsbedingungen akzeptiert
        /// </summary>
        public bool AgbAccepted
        {
            get => _agbAccepted;
            set
            {
                _agbAccepted = value;
                CmdLogin.CanExecute();
            }
        }

        /// <summary>
        ///     Fehlertext
        /// </summary>
        public string ErrorText { get; set; } = string.Empty;

        /// <summary>
        ///     Login Command
        /// </summary>
        public VmCommand CmdLogin { get; set; } = null!;

        /// <summary>
        ///     AgbLink öffnen
        /// </summary>
        public VmCommand CmdAgb { get; set; } = null!;

        /// <summary>
        ///     Login Button wird angezeigt
        /// </summary>
        public bool IsLoginButtonVisible { get; set; }

        /// <summary>
        ///     Verneinung ob login button sichtbar ist
        /// </summary>
        [DependsOn(nameof(IsLoginButtonVisible))]
        public bool NotIsLoginButtonVisible => !IsLoginButtonVisible;

        #endregion

        /// <summary>
        ///     Wird aufgerufen sobald die View initialisiert wurde
        /// </summary>
        /// <param name="args">Die argumente</param>
        /// <returns>Einen Task</returns>
        public override Task OnActivated(object? args = null)
        {
            ErrorText = string.Empty;
            IsLoginButtonVisible = true;
            return base.OnActivated(args);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdLogin = new VmCommand(ResViewLogin.Login, () => { Login(true); }, () => AgbAccepted);

            CmdAgb = new VmCommand(ResViewFirstLogin.CmdAgb, async () => { await OpenWebLink(Constants.AgbLink).ConfigureAwait(true); });
        }

        /// <summary>
        ///     Login
        /// </summary>
        private async void Login(bool tryWithUi = false)
        {
            IsBusy = true;
            BusyContent = ResViewLogin.StartingMsLogin;

            if (Dc.AutoConnect == false)
            {
                await Dc.OpenConnection(true).ConfigureAwait(true);
            }


            IsBusy = false;

            await DeviceInfoUpdate().ConfigureAwait(true);

            if (Dc.DcExUserSettings.DataSource != EnumDcDataSource.FromServer)
            {
                await Dc.DcExUserSettings.WaitDataFromServerAsync().ConfigureAwait(true);
            }

            CmdHome.IsSelected = true;
        }
    }
}