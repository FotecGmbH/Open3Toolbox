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
using Biss.Apps.Interfaces;
using Biss.Apps.ViewModel;
using Exchange.Resources;

// ReSharper disable once CheckNamespace
namespace BaseApp.ViewModel
{
    /// <summary>
    ///     <para>Viewmodel für die Settings von MS To-Do Liste</para>
    ///     Klasse VmSettingsMicrosoftToDo. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmSettingsMicrosoftToDo : VmProjectBase
    {
        /// <summary>
        ///     Design Instanz für XAML d:DataContext="{x:Static viewmodels:VmSettingsMicrosoftToDo.DesignInstance}"
        /// </summary>
        public static VmSettingsMicrosoftToDo DesignInstance = new VmSettingsMicrosoftToDo();

        /// <summary>
        ///     VmSettingsMicrosoftToDo
        /// </summary>
        public VmSettingsMicrosoftToDo() : base(ResViewSettingsMicrosoftToDo.Title)
        {
            IsBusy = true;
        }

        #region Properties

        /// <summary>
        ///     Save Command
        /// </summary>
        public VmCommand CmdSave { get; set; } = null!;

        #endregion

        /// <summary>
        ///     Wird aufgerufen sobald die View initialisiert wurde
        /// </summary>
        /// <param name="args">Die Argumente</param>
        /// <returns>Einen Task</returns>
        public override Task OnActivated(object? args = null)
        {
            Dc.DcExUserSettings.Data.PropertyChanged -= Data_PropertyChanged;
            Dc.DcExUserSettings.Data.PropertyChanged += Data_PropertyChanged;
            Dc.DcExUserSettings.Update();
            IsBusy = false;
            return base.OnActivated(args);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdSave = new VmCommand(ResViewSettingsMicrosoftToDo.Cmd_Save, async () =>
            {
                var result = await Dc.DcExUserSettings.StoreData().ConfigureAwait(true);

                if (!result.DataOk)
                {
                    await MsgBox!.Show(ResViewSettings.MsgBox_TextSaveError, ResViewSettings.MsgBox_CaptionSaveError, icon: VmMessageBoxImage.Error)
                        .ConfigureAwait(true);
                    return;
                }

                await MsgBox!.Show(ResViewSettings.MsgBox_TextSaveSuccess, ResViewSettings.MsgBox_CaptionSaveSuccess).ConfigureAwait(true);

                await Nav.Back().ConfigureAwait(true);
            }, CanExecuteCmdSave);
        }

        /// <summary>
        ///     Wird aufgerufen wenn sich eine Eigenschaft geändert hat
        /// </summary>
        /// <param name="sender">Die Eigenschaft</param>
        /// <param name="e">Die Argumente des Events</param>
        private void Data_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CmdSave.CanExecute();
        }

        /// <summary>
        ///     Validierung ob CommandLogin gedrückt werden kann
        /// </summary>
        /// <returns>Ob valide oder nicht</returns>
        private bool CanExecuteCmdSave()
        {
            return !string.IsNullOrWhiteSpace(Dc.DcExUserSettings.Data.MsTodoListName) || !Dc.DcExUserSettings.Data.MsTodoListActive;
        }
    }
}