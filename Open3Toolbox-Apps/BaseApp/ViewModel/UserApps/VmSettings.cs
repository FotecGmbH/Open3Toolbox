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
using Exchange.Resources;

// ReSharper disable once CheckNamespace
namespace BaseApp.ViewModel
{
    /// <summary>
    ///     <para>Viewmodel für die Einstellungen</para>
    ///     Klasse VmSettings. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmSettings : VmProjectBase
    {
        /// <summary>
        ///     Design Instanz für XAML d:DataContext="{x:Static viewmodels:VmSettings.DesignInstance}"
        /// </summary>
        public static VmSettings DesignInstance = new VmSettings();

        /// <summary>
        ///     VmSettings
        /// </summary>
        public VmSettings() : base(ResViewSettings.Title)
        {
            IsBusy = true;
            IfLastOnStackNavToMain = true;
        }

        #region Properties

        /// <summary>
        ///     Navigiere zu den Planarbeitszeiteinstellungen Command
        /// </summary>
        public VmCommandSelectable CmdGoToPlanWorkTimeSettings { get; set; } = null!;

        /// <summary>
        ///     Navigiere zu den Push Nachrichten Einstellungen Command
        /// </summary>
        public VmCommandSelectable CmdGoToPushMessageSettings { get; set; } = null!;

        /// <summary>
        ///     Navigiere zu den MS To Do Einstellungen Command
        /// </summary>
        public VmCommandSelectable CmdGoToMsToDoSettings { get; set; } = null!;

        /// <summary>
        ///     Commanf für Eisntellungen
        /// </summary>
        public VmCommandSelectable CmdCommonSettings { get; set; } = null!;

        #endregion

        /// <summary>
        ///     Wird aufgerufen sobald die View initialisiert wurde
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override Task OnActivated(object? args = null)
        {
            IsBusy = false;
            return base.OnActivated(args);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdCommonSettings = new VmCommandSelectable("Individualisieren", async () =>
            {
                await Nav.ToViewWithResult("ViewSettingsCommon").ConfigureAwait(true);
                _ = Task.Run(async () =>
                {
                    await Task.Delay(700).ConfigureAwait(true);
                    CmdCommonSettings.IsSelected = false;
                });
            }, glyph: "\uE918", toolTip: "Individuelle Einstellungen");

            CmdGoToPlanWorkTimeSettings = new VmCommandSelectable(ResViewSettings.Cmd_PlannedWorkingTime, async () =>
            {
                await Nav.ToViewWithResult("ViewSettingsWorkTime").ConfigureAwait(true);
                _ = Task.Run(async () =>
                {
                    await Task.Delay(700).ConfigureAwait(true);
                    CmdGoToPlanWorkTimeSettings.IsSelected = false;
                });
            }, glyph: "\uE97B", toolTip: "Täglich geplante Arbeitszeiten");

            CmdGoToPushMessageSettings = new VmCommandSelectable(ResViewSettings.Cmd_PushMessages, async () =>
            {
                await Nav.ToViewWithResult("ViewSettingsPush").ConfigureAwait(true);
                _ = Task.Run(async () =>
                {
                    await Task.Delay(700).ConfigureAwait(true);
                    CmdGoToPushMessageSettings.IsSelected = false;
                });
            }, glyph: "\uE9A6", toolTip: "Push Nachrichten verwalten");

            CmdGoToMsToDoSettings = new VmCommandSelectable(ResViewSettings.Cmd_MsToDo, async () =>
            {
                await Nav.ToViewWithResult("ViewSettingsMicrosoftToDo").ConfigureAwait(true);
                _ = Task.Run(async () =>
                {
                    await Task.Delay(700).ConfigureAwait(true);
                    CmdGoToMsToDoSettings.IsSelected = false;
                });
            }, "Einstellungen für Microsoft ToDo");
        }
    }
}