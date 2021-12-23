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
using Biss.Apps.Interfaces;
using Biss.Apps.ViewModel;
using Exchange.DataConnector;
using Exchange.Resources;

// ReSharper disable once CheckNamespace
namespace BaseApp.ViewModel
{
    /// <summary>
    ///     <para>Viewmodel für die "Ich"-Ansicht</para>
    ///     Klasse VmMe. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmMe : VmProjectBase
    {
        /// <summary>
        ///     Design Instanz für XAML d:DataContext="{x:Static viewmodels:VmMe.DesignInstance}"
        /// </summary>
        public static VmMe DesignInstance = new VmMe();

        /// <summary>
        ///     VmMe
        /// </summary>
        public VmMe() : base(ResViewMe.Title)
        {
            IsBusy = true;
            IfLastOnStackNavToMain = true;
        }

        #region Properties

        /// <summary>
        ///     Abmelden/Logout Command
        /// </summary>
        public VmCommand CmdLogout { get; set; } = null!;

        /// <summary>
        ///     Account löschen Command
        /// </summary>
        public VmCommand CmdDeleteAccount { get; set; } = null!;

        #endregion

        /// <summary>
        ///     Wird aufgerufen sobald die View initialisiert wurde
        /// </summary>
        /// <param name="args">Argumente</param>
        /// <returns>Einen Task</returns>
        public override async Task OnActivated(object? args = null)
        {
            IsBusy = false;
            await base.OnActivated(args).ConfigureAwait(true);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdLogout = new VmCommand(ResViewMe.Cmd_Logout, Logout);

            CmdDeleteAccount = new VmCommand(ResViewMe.Cmd_DeleteAccount, async () =>
            {
                var result = await MsgBox!
                    .Show(ResViewMe.MsgBox_SafetyQuestionText, ResViewMe.MsgBox_SafetyQuestionCaption, VmMessageBoxButton.YesNo, VmMessageBoxImage.Warning)
                    .ConfigureAwait(true);

                if (result == VmMessageBoxResult.No)
                {
                    return;
                }

                var r = await Dc.Send(EnumDcCommonCommand.DeleteAccount, string.Empty).ConfigureAwait(true);

                if (!r.Ok)
                {
                    await MsgBox!.Show(ResViewEditDay.MsgBoxText_SaveError, ResViewEditDay.MsgBoxCaption_SaveError, VmMessageBoxButton.Ok, VmMessageBoxImage.Error)
                        .ConfigureAwait(true);
                }
            });
        }
    }
}