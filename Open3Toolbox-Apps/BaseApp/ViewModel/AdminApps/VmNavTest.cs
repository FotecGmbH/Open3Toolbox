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


namespace BaseApp.ViewModel.AdminApps
{
    /// <summary>
    ///     <para>Test Web</para>
    ///     Klasse ViewModelUserAccount. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmNavTest : VmProjectBase
    {
        /// <summary>
        ///     Test Web
        /// </summary>
        public VmNavTest() : base("?")
        {
            ViewResult = "???";
        }

        #region Properties

        /// <summary>
        ///     Text
        /// </summary>
        public string MyText { get; set; } = string.Empty;

        /// <summary>
        ///     Command
        /// </summary>
        public VmCommand CmdOk { get; private set; } = null!;

        #endregion


        /// <summary>View wurde erzeugt</summary>
        public override Task OnActivated(object? args = null)
        {
            if (args is string title)
            {
                PageTitle = title;
            }

            return base.OnActivated(args);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdOk = new VmCommand("Ok", async () =>
            {
                if (!string.IsNullOrEmpty(MyText))
                {
                    ViewResult = MyText;
                }

                await Nav!.Back().ConfigureAwait(true);
            });
        }
    }
}