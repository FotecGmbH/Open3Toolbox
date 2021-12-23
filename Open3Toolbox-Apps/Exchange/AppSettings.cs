// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System.Collections.Generic;
using Biss.Apps.Components;
using BISS.Apps.Connectivity.Interfaces;
using BISS.Apps.Connectivity.Sa;
using Biss.Apps.Interfaces;
using Biss.Apps.Model;
using Biss.Apps.Push.Interfaces;
using Biss.Dc.Client;
using Exchange.Resources;

namespace Exchange
{
    public class AppSettings :
        IAppSettings,
        IAppSettingsNavigation,
        IAppSettingsFiles,
        IAppSettingsPush,
        IAppSettingConnectivity
    {
        private static AppSettings _current;

        public AppSettings()
        {
            BranchName = "dev";
            AppConfigurationConstants = 2;
            PackageName = "at.ak.open3toolbox.dev";
            AppVersion = "0.5.0.0";
            AppName = "Open3 Toolbox";
            AssemblyDescription = "Open3 Toolbox";
            AssemblyConfiguration = "";
            AssemblyCompany = "FOTEC Forschungs- und Technologietransfer GmbH";
            AssemblyCopyright = "Copyright © 1998-2021 FOTEC - Forschungs- und Technologietransfer GmbH";
            AssemblyTrademark = "";
            AssemblyCulture = "";
            DefaultUserId = -1;
            LanguageContent = null!;
            ProjectWorkUserFolder = "Open3 Toolbox";
            BaseNavigator = null!;
            DefaultBackText = ResView.Command_Back;
            DefaultViewAssembly = "BaseApp.View";
            DefaultViewNamespace = "BaseApp.View.";
            Master = null!;
            MasterDetail = null!;
            NavArgsHelper = null!;
            Navigation = null!;
            NavigationManager = null!;
            QuitApplication = null!;
            Shell = null!;
            BaseFiles = null!;
            NotificationChannelId = "DefaultId";
            NotificationChannelName = "DefaultChannel";
            Platform = null!;
            Topics = new List<string> {"default"};
            ;
            DcAppCache = null!;
            DcAppStorage = null!;
            DcClient = null!;
            DcEnabled = false;
            DcSignalHost = "https://localhost:44369/";
            DcUseUser = true;
            SaApiHost = "http://localhost:44369/api/";
            SaClient = null!;
            SaEnabled = false;
        }

        #region Properties

        public string BranchName { get; set; }
        public int AppConfigurationConstants { get; set; }
        public string PackageName { get; set; }
        public string AppVersion { get; set; }
        public string AppName { get; set; }
        public string AssemblyDescription { get; set; }
        public string AssemblyConfiguration { get; set; }
        public string AssemblyCompany { get; set; }
        public string AssemblyCopyright { get; set; }
        public string AssemblyTrademark { get; set; }
        public string AssemblyCulture { get; set; }

        #region IAppSettingsFiles

        public VmFiles BaseFiles { get; set; }

        #endregion

        #endregion

        public static AppSettings Current()
        {
            if (_current == null)
            {
                _current = new AppSettings();
            }

            return _current;
        }

        #region IAppSettings

        public int DefaultUserId { get; set; }
        public ExLanguageContent LanguageContent { get; set; }
        public string ProjectWorkUserFolder { get; set; }

        #endregion

        #region IAppSettingsNavigation

        public VmNavigator BaseNavigator { get; set; }
        public string DefaultBackText { get; set; }
        public string DefaultViewAssembly { get; set; }
        public string DefaultViewNamespace { get; set; }
        public object Master { get; set; }
        public object MasterDetail { get; set; }
        public INavArgsHelper NavArgsHelper { get; set; }
        public object Navigation { get; set; }
        public object NavigationManager { get; set; }
        public IQuitApplication? QuitApplication { get; set; }
        public object Shell { get; set; }

        #endregion

        #region IAppSettingsPush

        public string NotificationChannelId { get; set; }
        public string NotificationChannelName { get; set; }
        public IPlatformPush Platform { get; set; }
        public List<string> Topics { get; set; }

        #endregion

        #region IAppSettingConnectivity

        public IDcClientCacheStorage DcAppCache { get; set; }
        public IDcClientInfoStorage DcAppStorage { get; set; }
        public DcDataRoot DcClient { get; set; }
        public bool DcEnabled { get; set; }
        public string DcSignalHost { get; set; }
        public bool DcUseUser { get; set; }
        public string SaApiHost { get; set; }
        public RestAccessBase SaClient { get; set; }
        public bool SaEnabled { get; set; }

        #endregion
    }
}