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
using Biss.Apps.Base;
using Biss.Apps.ViewModel;
using Exchange.Resources.ResAdminApps;

namespace BaseApp.ViewModel.AdminApps
{
    /// <summary>
    ///     <para>Viewmodel für Login View</para>
    ///     Klasse VmAdminLogin. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmAdminLogin : VmProjectBase
    {
        /// <summary>
        ///     VmAdminLogin
        /// </summary>
        public VmAdminLogin() : base(ResViewAdminLogin.Title)
        {
            IsBusy = false;
        }

        #region Properties

        /// <summary>
        ///     Command für Login
        /// </summary>
        public VmCommand CmdLogin { get; set; } = null!;

        /// <summary>
        ///     Password im Klartext für Binding.
        /// </summary>
        public string PasswordPlaintext { get; set; } = string.Empty;

        /// <summary>
        ///     Login Daten
        /// </summary>
        public string LoginName { get; set; } = string.Empty;

        #endregion

        /// <summary>
        ///     Wird aufgerufen sobald die View initialisiert wurde
        /// </summary>
        /// <param name="args">Argumente</param>
        /// <returns>Task</returns>
        public override Task OnActivated(object? args = null)
        {
            IsBusy = false;
            return Task.CompletedTask;
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdLogin = new VmCommand(ResViewAdminLogin.Lbl_CmdLogin, async () =>
            {
                IsBusy = true;

                var userId = await Dc.CheckUserLoginName(LoginName).ConfigureAwait(true);

                var successLogin = false;

                if (userId.HasValue)
                {
                    successLogin = await Dc.CheckUserPassword(userId.Value, AppCrypt.CumputeHash(PasswordPlaintext)).ConfigureAwait(true);
                }

                IsBusy = false;

                if (successLogin)
                {
                    LaunchFirstView();
                }
                else
                {
                    _ = MsgBox!.Show(ResViewAdminLogin.Text_LoginNoSuccess, ResViewAdminLogin.Caption_LoginNoSuccess).ConfigureAwait(false);
                }
            }, CanExecuteCmdLogin);
        }

        /// <summary>
        ///     Validierung ob CommandLogin gedrückt werden kann
        /// </summary>
        /// <returns>Ob command gedrückt werden darf</returns>
        private bool CanExecuteCmdLogin()
        {
            return !string.IsNullOrWhiteSpace(LoginName) &&
                   !string.IsNullOrWhiteSpace(PasswordPlaintext);
        }
    }
}