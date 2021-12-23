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
using Exchange.Resources;


namespace BaseApp.ViewModel.AdminApps
{
    /// <summary>
    ///     <para>Admin App VmMain</para>
    ///     Klasse ViewModelUserAccount. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmAdminMain : VmProjectBase
    {
        /// <summary>
        ///     ViewModel Template
        /// </summary>
        public VmAdminMain() : base(ResViewMain.Title, subTitle: ResViewMain.Subtitle)
        {
            IsBusy = true;
            BusyContent = "Lade Daten ...";
        }


        /// <summary>View wurde erzeugt</summary>
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
        }
    }
}