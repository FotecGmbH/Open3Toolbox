// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       AdminWebApp.Client
// 
// Released under MIT

using System;
using BaseApp;
using BaseApp.DataConnector;
using Biss.Apps.Base;
using Biss.Apps.Blazor;
using BISS.Apps.Connectivity.Blazor;
using Biss.Log.Producer;
using Exchange;
using Exchange.Resources;
using Microsoft.Extensions.Logging;

namespace AdminWebApp.Client
{
    /// <summary>
    ///     <para>Main</para>
    ///     Klasse Main.cs (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public static class Main
    {
        /// <summary>
        ///     Init
        /// </summary>
        public static void Init()
        {
            Logging.Init(l => l.AddDebug().SetMinimumLevel(LogLevel.Trace));

            Logging.Log.LogInfo("Blazor: Init started");

            AppSettings.Current().DefaultViewNamespace = "AdminWebApp.Client.Pages.";
            AppSettings.Current().DefaultViewAssembly = "AdminWebApp.Client";

            BissInitializer.Initialize(AppSettings.Current(), new Language());
            BlazorConnectivityExtensions.BissUseDc(null, AppSettings.Current(), new DcProjectBase());

            VmProjectBase.InitializeApp(EnumAppType.Admin).ConfigureAwait(true);

            XamarinDeviceInfo.DeviceHardwareId = Guid.NewGuid().ToString();

            Logging.Log.LogInfo("Blazor: Init finished");
        }
    }
}