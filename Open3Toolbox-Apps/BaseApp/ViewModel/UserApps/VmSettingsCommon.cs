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
using BISS.Apps.Connectivity.Dc;
using Biss.Apps.Interfaces;
using Biss.Apps.ViewModel;
using Exchange.Model;
using Exchange.Resources;

// ReSharper disable once CheckNamespace
namespace BaseApp.ViewModel
{
    /// <summary>
    ///     <para>Control Test</para>
    ///     Klasse VmTestControls. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmSettingsCommon : VmProjectBase
    {
        /// <summary>
        ///     Design Instanz für XAML d:DataContext="{x:Static viewmodels:VmTestControls.DesignInstance}"
        /// </summary>
        public static VmSettingsCommon DesignInstance = new VmSettingsCommon();

        /// <summary>
        ///     VmTestControls
        /// </summary>
        public VmSettingsCommon() : base("Individualisieren")
        {
            IsBusy = true;
            ShowFooter = false;
            ViewResult = false;
        }

        #region Properties

        /// <summary>
        ///     Command für speichern
        /// </summary>
        public VmCommand CmdStore { get; set; } = null!;

        /// <summary>
        ///     Falls entry focus einen Fehler hat
        /// </summary>
        public bool EntryFocusHasError { get; set; }

        /// <summary>
        ///     Die Fehler Meldung von entry focus
        /// </summary>
        public string EntryFocusError { get; set; } = string.Empty;

        #endregion

        /// <summary>
        ///     Wird aufgerufen sobald die View initialisiert wurde
        /// </summary>
        /// <param name="args">Die Argumente</param>
        /// <returns>Einen Task</returns>
        public override async Task OnActivated(object? args = null)
        {
            IsBusy = false;

            await Dc.DcExUserSettings.WaitDataFromServerAsync().ConfigureAwait(true);

            CheckSaveBehavior = new CheckSaveDcBehavior<ExUser>(Dc.DcExUserSettings);
            Dc.DcExUserSettings.DataChangedEvent += (sender, eventArgs) => { CmdStore.CanExecute(); };
            Dc.DcExUserSettings.Data.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == nameof(Dc.DcExUserSettings.Data.FocusTimeUiName))
                {
                    CmdStore.CanExecute();
                }
            };

            await base.OnActivated(args).ConfigureAwait(true);
        }

        /// <summary>View wurde inaktiv</summary>
        /// <returns>Einen Task</returns>
        public override Task OnDisappearing(IView view)
        {
            if (CheckSaveBehavior != null)
            {
                Dc.DcExUserSettings.EndEdit(true);
            }

            return base.OnDisappearing(view);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdStore = new VmCommand(ResViewEditDay.Cmd_Save, async () =>
            {
                var r = await Dc.DcExUserSettings.StoreData(true).ConfigureAwait(true);
                if (r.DataOk)
                {
                    ViewResult = true;
                    Dc.DcExUserSettings.EndEdit();
                    CheckSaveBehavior = null;
                    await Nav!.Back().ConfigureAwait(true);
                }
                else
                {
                    await MsgBox.Show(ResViewEditDay.MsgBoxText_SaveError).ConfigureAwait(true);
                }
            }, CanExecuteOk);
        }

        /// <summary>
        ///     Ob ausgeführt werden kann
        /// </summary>
        /// <returns>Ob ausgeführt werden kann</returns>
        private bool CanExecuteOk()
        {
            if (CheckSaveBehavior == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(Dc.DcExUserSettings.Data.FocusTimeUiName))
            {
                EntryFocusError = "Bezeichnung darf nicht leer sein!";
                EntryFocusHasError = true;
                return false;
            }

            if (Dc.DcExUserSettings.Data.FocusTimeUiName.Length > 15)
            {
                EntryFocusError = "Bezeichnung ist zu lang! Bitte maximal 15 Zeichen.";
                EntryFocusHasError = true;
                return false;
            }

            EntryFocusHasError = false;
            return CheckSaveBehavior.Check();
        }
    }
}