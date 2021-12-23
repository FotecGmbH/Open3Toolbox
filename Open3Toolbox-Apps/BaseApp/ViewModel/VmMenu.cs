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
using System.IO;
using Biss.Apps.ViewModel;
using Biss.Collections;
using Exchange.Resources;

namespace BaseApp.ViewModel
{
    /// <summary>
    ///     View Model Menü Klasse
    /// </summary>
    public class VmMenu : VmProjectBase
    {
        /// <summary>
        ///     ViewModel Menü
        /// </summary>
        public VmMenu() : base(ResViewMenu.Title)
        {
        }

        #region Properties

        public static VmMenu DesignInstance => new VmMenu();

        /// <summary>
        ///     Alle Menüeinträge für seitliches Menü
        /// </summary>
        public ObservableCollectionSelectable<VmCommandSelectable> CmdAllMenuCommands { get; } = new ObservableCollectionSelectable<VmCommandSelectable>();

        public ObservableCollectionSelectable<VmCommandSelectable> CmdShadowList { get; } = new ObservableCollectionSelectable<VmCommandSelectable>();

        /// <summary>
        ///     Bild
        /// </summary>
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public Stream Image => Images.ReadImageAsStream(EnumEmbeddedImage.Logo3_png);
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        #endregion

        /// <inheritdoc />
        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdAllMenuCommands.Clear();
            CmdShadowList.Clear();

            if (CurrentAppType == EnumAppType.User)
            {
                CmdShadowList.Add(CmdMe);
                CmdShadowList.Add(CmdHome);

                CmdAllMenuCommands.Add(CmdMe);
            }
            else if (CurrentAppType == EnumAppType.Admin)
            {
                CmdAllMenuCommands.Add(CmdAdminHome);

                // Config Tool
                CmdAllMenuCommands.Add(CmdConfigTool);

                // Sensor Statistics
                CmdAllMenuCommands.Add(CmdSensorStatistics);
            }
        }
    }
}