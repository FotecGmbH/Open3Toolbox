// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       BaseApp.View
// 
// Released under MIT

using System;
using System.Threading;
using System.Threading.Tasks;
using BaseApp.Connectivity;
using BaseApp.DataConnector;
using Biss.AppConfiguration;
using Biss.Apps.Base;
using BISS.Apps.Connectivity.Dc;
using BISS.Apps.Connectivity.XF;
using Biss.Apps.Interfaces;
using Biss.Apps.XF;
using Biss.Log.Producer;
using Exchange;
using Exchange.Resources;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;
using Syncfusion.Licensing;
using Xamarin.Essentials;
using Xamarin.Forms;
using Device = Xamarin.Forms.Device;

namespace BaseApp
{
    public partial class App : Application
    {
        /// <summary>
        ///     Nie aufrufen! Wird nur von Xamarin.Forms Previewer benötigt!
        /// </summary>
        public App()
        {
            ThreadHelper.Initialize(Environment.CurrentManagedThreadId);
            InitializeComponent();
        }

        /// <summary>
        ///     Xamarin BISS MvvM initialisieren und starten IBissAppPlattform
        /// </summary>
        public App(IBissAppPlattform plattform)
        {
            InitializeComponent();
            AppActions.OnAppAction += OnAppAction;
            // Syncfusion Key für 18.4.0.48
            SyncfusionLicenseProvider.RegisterLicense("NDE1NDYwQDMxMzgyZTM0MmUzMFBybU96VjhlR2p0Um9DNVU1MVJoME4zTmdnZFV5NlpZUnhmR2o0dURYQUU9");

            if (this.UseBissXf(plattform, AppSettings.Current(), new Language(), typeof(ViewMenu)))
            {
                //Connectivity
                this.BissUseDc(AppSettings.Current(), new DcProjectBase());
                this.BissUseSa(AppSettings.Current(), new SaProjectBase());

                try
                {
                    var droid = Constants.AppConfiguration.CurrentBuildType == EnumCurrentBuildType.CustomerRelease ? "c7f38eb2-5145-4794-8f2e-f938651caf2f"
                        : Constants.AppConfiguration.CurrentBuildType == EnumCurrentBuildType.CustomerBeta ? "9e1312c8-8b46-4690-a66b-95f2e605179e"
                        : "db5f4261-5a2e-46a9-84d3-688eb5c3c4ab";
                    var iOS = Constants.AppConfiguration.CurrentBuildType == EnumCurrentBuildType.CustomerRelease ? "57a720a3-9137-44fa-92e9-371012f3cb19"
                        : Constants.AppConfiguration.CurrentBuildType == EnumCurrentBuildType.CustomerBeta ? "9abcab1f-64ab-46cf-b802-57dc859a80e9"
                        : "56c327a3-de71-4651-b15d-5299694b4675";

                    var secrets = $"android={droid};ios={iOS}";

                    AppCenter.Start(secrets, typeof(Analytics), typeof(Crashes));
                }
                catch (Exception e)
                {
                    Logging.Log.LogError($"{e}");
                }

                VmProjectBase.InitializeApp().ConfigureAwait(true);

                VmProjectBase.Open = DependencyService.Get<IOpenHelper>();

                VmProjectBase.LaunchFirstView();
            }
            else
            {
                throw new Exception();
            }
        }

        private void OnAppAction(object sender, AppActionEventArgs e)
        {
            if (Current != this && Current is App app)
            {
                AppActions.OnAppAction -= app.OnAppAction;
                return;
            }

            Device.BeginInvokeOnMainThread(() =>
            {
                if (!string.IsNullOrWhiteSpace(e.AppAction.Id))
                {
                    VmProjectBase.LaunchFirstView(e.AppAction.Id);
                }
            });
        }

        #region OnStart/Sleep/Resume

        private CancellationTokenSource _sleepToken = new CancellationTokenSource();

        protected override async void OnStart()
        {
            // Handle when your app starts
            base.OnStart();
            Logging.Log.LogTrace("APP.xaml.cs: OnStart");
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            base.OnSleep();
            Logging.Log.LogTrace("APP.xaml.cs: OnSleep");
            _sleepToken = new CancellationTokenSource();
            Task.Run(async () =>
            {
                await Task.Delay(20000, _sleepToken.Token);
                if (!_sleepToken.IsCancellationRequested)
                {
                    Logging.Log.LogInfo("APP.xaml.cs: Close DC Conntection");
                    var dc = BcDataConnectorExtensions.BcDataConnector(null!)?.GetDc<DcProjectBase>();
                    dc?.CloseConnection(true);
                }
            });
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            base.OnResume();
            Logging.Log.LogTrace("APP.xaml.cs: OnResume");

            _sleepToken.Cancel();
            var dc = BcDataConnectorExtensions.BcDataConnector(null!)?.GetDc<DcProjectBase>();
            if (dc != null)
            {
                if (dc.AutoConnect == false)
                {
                    dc.OpenConnection(true);
                }
            }
        }

        #endregion
    }
}