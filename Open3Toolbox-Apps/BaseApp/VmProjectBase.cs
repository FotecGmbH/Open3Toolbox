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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using BaseApp.DataConnector;
using BaseApp.ViewModel;
using Biss.Apps;
using Biss.Apps.Base;
using Biss.Apps.Components.Navigation;
using BISS.Apps.Connectivity;
using BISS.Apps.Connectivity.Dc;
using Biss.Apps.Interfaces;
using Biss.Apps.Push;
using Biss.Apps.ViewModel;
using Biss.Common;
using Biss.Dc.Core;
using Biss.Log.Producer;
using Biss.Time;
using Exchange;
using Exchange.DataConnector;
using Exchange.Enum;
using Exchange.Model;
using Exchange.Resources;
using Exchange.Resources.ResAdminApps;
using Exchange.Resources.ResConfigurationTool;
using Microsoft.Extensions.Logging;
using PropertyChanged;
using Xamarin.Essentials;

namespace BaseApp
{
    /// <summary>
    ///     Enum für App Type
    /// </summary>
    public enum EnumAppType
    {
        User = 0,
        Admin
    }

    public class DesignVmBase : VmProjectBase
    {
        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        /// <param name="pageTitle"></param>
        /// <param name="args"></param>
        /// <param name="subTitle"></param>
        public DesignVmBase(string pageTitle, object? args = null, string subTitle = "") : base(pageTitle, args, subTitle)
        {
        }

        #region Properties

        /// <summary>
        ///     Instanz für Design Zwecke
        /// </summary>
        public static DesignVmBase DesignInstance => new DesignVmBase("");

        #endregion
    }

    /// <summary>
    ///     <para>DESCRIPTION</para>
    ///     Klasse VmBaseData. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmBaseData : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        ///     Aktuelle Zeit für Binding
        /// </summary>
        public string CurrentTime { get; set; } = string.Empty;

        #endregion

        #region Interface Implementations

        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged = null!;

        #endregion
    }

    /// <summary>
    ///     <para>Basis View Model projektspezifisch</para>
    ///     Klasse ViewModelBase. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public abstract class VmProjectBase : VmBase
    {
        private static bool _deviceDataUpdatedInSession;
        private static Stream _defaultImage = null!;
        private static BissTimer? _timer;
        private static VmMenu _menu = null!;
        protected static bool _firstLaunch = false;

        public static VmBaseData BaseDataStatic = new VmBaseData();


        /// <summary>
        ///     Wann wurde (wenn aktiv) dem Benuter der Wartungstext angezeigt
        /// </summary>
        public static DateTime? MaintenanceShowOn = null;

        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        /// <param name="pageTitle"></param>
        /// <param name="args"></param>
        /// <param name="subTitle"></param>
        protected VmProjectBase(string pageTitle, object? args = null, string subTitle = "") : base(pageTitle, args, subTitle)
        {
            if (_defaultImage == null!)
            {
                _defaultImage = Images.ReadImageAsStream(EnumEmbeddedImage.Logo_png);
            }
        }

        #region Properties

        /// <summary>
        ///     i im Header sichtbar?
        /// </summary>
        public bool InfoInHeaderVisible { get; set; } = true;

        public static EnumAppType CurrentAppType { get; set; } = EnumAppType.User;
        public VmBaseData BaseData => BaseDataStatic;

        /// <summary>
        ///     Footer in der View Anzeigen
        /// </summary>
        public bool ShowFooter { get; set; } = true;

        /// <summary>
        ///     Werden Titel und Sub-Titel angezeigt
        /// </summary>
        public bool ShowTitle { get; set; } = false;

        /// <summary>
        ///     Wird User Status und Zeit angezeigt
        /// </summary>
        [DependsOn(nameof(ShowTitle))]
        public bool ShowUserStateAndTime => !ShowTitle;

        /// <summary>
        ///     Zugriff auf das Hauptmenü
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        public VmMenu MainMenu => GetVmBaseStatic();
#pragma warning restore CA1822 // Mark members as static

        /// <summary>
        ///     Zugriff auf Push Komponente.
        /// </summary>
        public static BcPush Push => PushExtension.BcPush();

        /// <summary>
        ///     Bild
        /// </summary>
#pragma warning disable CA1822 // Mark members as static
        public Stream Image => _defaultImage;
#pragma warning restore CA1822 // Mark members as static

        /// <summary>
        ///     Data Connector
        /// </summary>
        public DcProjectBase Dc => this.GetDc<DcProjectBase>();

        #region Open

        /// <summary>
        ///     Wird von der View gesetzt.
        /// </summary>
        public static IOpenHelper Open { get; set; }

        #endregion

        public bool ShowNotConnected
        {
            get => _showNotConnected;
            set => _showNotConnected = value;
        }

        public bool IfLastOnStackNavToMain { get; set; }

        #endregion

        /// <summary>
        ///     Zugriff auf das Hauptmenü
        /// </summary>
        public static VmMenu GetVmBaseStatic()
        {
            if (_menu == null!)
            {
                _menu = new VmMenu();
            }

            return _menu;
        }

        /// <summary>
        ///     View wurde wieder aktiv unbedingt beim überschreiben auch base. aufrufen!
        /// </summary>
        /// <returns></returns>
        public override Task OnAppearing(IView view)
        {
            if (view != null)
            {
                var viewName = view.GetType().Name;

                var data = new ExViewState
                           {
                               CurrentDateTime = DateTime.Now,
                               IsAppearing = true,
                               ViewName = viewName,
                           };

                Dc.Send(EnumDcCommonCommand.TelemetryData, data);
            }

            return base.OnAppearing(view);
        }


        /// <summary>View wurde inaktiv</summary>
        /// <returns></returns>
        public override Task OnDisappearing(IView view)
        {
            if (view != null)
            {
                var viewName = view.GetType().Name;

                var data = new ExViewState
                           {
                               CurrentDateTime = DateTime.Now,
                               IsAppearing = false,
                               ViewName = viewName,
                           };
                Dc.Send(EnumDcCommonCommand.TelemetryData, data);
            }

            return base.OnDisappearing(view);
        }

        /// <summary>
        ///     Verbindung zum Server Prüfen
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CheckConnected()
        {
            if (Dc.ConnectionState != EnumDcConnectionState.Connected)
            {
                await MsgBox.Show("Leider besteht keine Verbindung zum Server!", "Verbindungsproblem").ConfigureAwait(true);
                ShowNotConnected = true;
                return false;
            }

            ShowNotConnected = false;
            return true;
        }

        /// <summary>
        ///     Welche View soll initial getartet weren
        /// </summary>
        public static async void LaunchFirstView(string args = "")
        {
            if (CManager == null || Dispatcher == null)
            {
                throw new ApplicationException("CManager, Dispatcher or Dc are null! Can not launch first View!");
            }

            await CManager!.InitHigh().ConfigureAwait(true);

            if (CurrentAppType == EnumAppType.User)
            {
                //_ = Task.Run(async () => { await CManager!.InitHigh().ConfigureAwait(true);}).ConfigureAwait(true);
                LaunchUserApp(args);
            }
            else if (CurrentAppType == EnumAppType.Admin)
            {
                await BcDataConnectorExtensions.BcDataConnector(null!)!.DataRoot!.OpenConnection(true).ConfigureAwait(true);

                //await CManager!.InitHigh().ConfigureAwait(true);
                _cmdAdminHome.IsSelected = true;
                //NavigatorExtensions.BcNavigation(null!)!.ToView("ViewAdminMain");
            }
        }


        /// <summary>Zurück gedrückt</summary>
        /// <returns></returns>
        public override bool OnBackButtonPressed(bool navigate = false)
        {
            if (IfLastOnStackNavToMain)
            {
                if (!_cmdHome.IsSelected)
                {
                    _cmdHome.IsSelected = true;
                    return true;
                }
            }

            return base.OnBackButtonPressed(navigate);
        }

        /// <summary>
        ///     Projekt Initialisieren
        /// </summary>
        public static Task InitializeApp(EnumAppType currentAppType = EnumAppType.User)
        {
            CurrentAppType = currentAppType;
            Logging.Log.LogTrace($"Init App - {currentAppType}");
            try
            {
                Logging.Log.LogTrace("Init App");
            }
            catch (Exception e)
            {
                Logging.Log.LogError($"Init App Error: {e}");
                throw;
            }

            return Task.CompletedTask;
        }


        /// <summary>
        ///     Daten über das aktuelle Device an die Cloud senden. Wird unter anderem für die Notifizierungen benötigt.
        ///     Sollte aufgerufen werden wenn der User eingeloggt ist. Wenn die App keine User unterstützt:
        ///     Db und Funktionen umbauen (das die Devices ohne User angelegt werden) ODER
        ///     Einen "ALLUSER" anlegen => wenn erslichtlich das eventuell mal ein Login hinzukommt
        /// </summary>
        public async Task DeviceInfoUpdate()
        {
            if (_deviceDataUpdatedInSession)
            {
                return;
            }

            if (!Dc.DeviceAndUserRegisteredLocal)
            {
                return;
            }

            Dc.DcExDevice.Data.DeviceHardwareId = XamarinDeviceInfo.DeviceHardwareId;
            // gehört noch getestet, evtl. umbau auf ein event der push komponente notwendig (GWe)
            if (DeviceInfo.Plattform == EnumPlattform.XamarinIos || DeviceInfo.Plattform == EnumPlattform.XamarinAndroid)
            {
                Dc.DcExDevice.Data.DeviceToken = Push.Token;
            }

            Dc.DcExDevice.Data.Plattform = DeviceInfo.Plattform;
            Dc.DcExDevice.Data.DeviceIdiom = DeviceInfo.DeviceIdiom;
            Dc.DcExDevice.Data.OperatingSystemVersion = DeviceInfo.OperatingSystemVersion;
            Dc.DcExDevice.Data.DeviceType = DeviceInfo.DeviceType;
            Dc.DcExDevice.Data.DeviceName = DeviceInfo.DeviceName;
            Dc.DcExDevice.Data.Manufacturer = DeviceInfo.Manufacturer;
            Dc.DcExDevice.Data.Model = DeviceInfo.Model;

            Dc.DcExDevice.Data.AppVersion = AppSettings.Current().AppVersion;
            Dc.DcExDevice.Data.PushTags = string.Empty;

            if (XamarinDeviceInfo.SupportsEssentialsDisplayInfo)
            {
                try
                {
                    Dc.DcExDevice.Data.ScreenResolution = DeviceDisplay.MainDisplayInfo.Width + " x " + DeviceDisplay.MainDisplayInfo.Height + " (" + DeviceDisplay.MainDisplayInfo.Density + ")";
                }
                catch
                {
                    // ios MainThread
                }
            }
            else
            {
                Dc.DcExDevice.Data.ScreenResolution = string.Empty;
            }


            Dc.DcExDevice.Data.TimeDifference = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);

            var storeRes = await Dc.DcExDevice.StoreData().ConfigureAwait(false);
            if (!storeRes.DataOk)
            {
                Logging.Log.LogError($"DeviceInfoUpdate Error({storeRes.ErrorType}): {storeRes.ServerExceptionText}");
                if (Debugger.IsAttached)
                {
                    Debugger.Break();
                }
            }
            else
            {
                Logging.Log.LogInfo("DeviceInfo Updated");
                _deviceDataUpdatedInSession = true;
            }
        }

        /// <summary>
        ///     Externes öffnen mit automatischer MessageBox bei Nichterfolg.
        /// </summary>
        /// <param name="openType">Zu öffnende Aktion</param>
        public void OpenWithFeedback(EnumOpenType openType)
        {
            bool showMsgBox;

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (Open != null)
            {
                showMsgBox = !Open.OpenExternal(openType);
            }
            else
            {
                showMsgBox = true;
            }

            if (showMsgBox)
            {
                MsgBox.Show(ResView.MsgBoxNotSupportedOnPlatform, ResView.MsgBoxNotSupportedOnPlatformCaption);

                if (openType == EnumOpenType.DoNotDisturb)
                {
                    string branchName = AppSettings.Current().BranchName.ToUpperInvariant();
                    var dev = branchName.Equals("DEV", StringComparison.InvariantCulture) || branchName.Equals("BETA", StringComparison.InvariantCulture);

                    if (dev)
                    {
                        MsgBox.Show("Do Not Disturb Settings konnten nicht geöffnet werden. Bitte um Testfeedback mit der Angabe von Smartphone und OS Version.");
                    }
                }
            }
        }

        /// <summary>
        ///     Logout wird aufgerufen
        /// </summary>
        public async void Logout()
        {
            if (Constants.SupportLogin)
            {
                AppCenter?.UpdateCurrentUser("", "");
            }

            _deviceDataUpdatedInSession = false;
            Dc.Logout();

            if (CurrentAppType == EnumAppType.User)
            {
                Nav.ToView("ViewLogin", showMenu: false);
            }
            else if (CurrentAppType == EnumAppType.Admin)
            {
                Nav.ToView("ViewAdminLogin", showMenu: true);
            }
        }

        public static async Task OpenWebLink(string link)
        {
            try
            {
                await Launcher.OpenAsync(new Uri(link)).ConfigureAwait(true);
            }
            catch (UriFormatException e)
            {
                Logging.Log.LogError($"{e}");

                await MsgBox!.Show(ResViewInfo.MsgBox_LinkErrorText, ResViewInfo.MsgBox_LinkErrorCaption, icon: VmMessageBoxImage.Error)
                    .ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                Logging.Log.LogError($"{e}");

                await MsgBox!.Show(ResViewInfo.MsgBox_LinkErrorText, ResViewInfo.MsgBox_LinkErrorCaption, icon: VmMessageBoxImage.Error)
                    .ConfigureAwait(false);
            }
        }

        protected void ResetSelectedButton(VmCommandSelectable button)
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(600).ConfigureAwait(true);
                button.IsSelected = false;
            });
        }

        /// <summary>
        ///     User App starten
        /// </summary>
        private static void LaunchUserApp(string args = "")
        {
            var dc = BcDataConnectorExtensions.BcDataConnector(null!)!.DataRoot;

            dc.CommonDataFromServerReceived += (sender, args) =>
            {
                var r = Enum.TryParse<EnumDcCommonCommandsClient>(args.Data.Key, true, out var t);
                if (!r)
                {
                    Logging.Log.LogWarning($"Dc Common Data from Server: {args.Data.Key}:{args.Data.Value}");
                    return;
                }

                switch (t)
                {
                    case EnumDcCommonCommandsClient.LogoutUser:
                        Dispatcher!.RunOnDispatcher(() =>
                        {
                            MsgBox!.Show(ResView.NeedReLogin, ResView.NeedReLoginHead);
                            GetVmBaseStatic().Logout();
                        });
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($"{nameof(t)}");
                }
            };

            if (_timer == null)
            {
                BaseDataStatic.CurrentTime = string.Format(CultureInfo.CurrentCulture, "{0:t}", DateTime.Now);
                _timer = new BissTimer(() => { BaseDataStatic.CurrentTime = string.Format(CultureInfo.CurrentCulture, "{0:t}", DateTime.Now); }, new TimeSpan(0, 0, 10));
                _timer.Start();
            }

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (Push != null)
            {
                Push.PushReceived += (sender, args) => { Toast.Show(args.Description, args.Title); };
            }

            //iOS Simulatur hack
            //await dc.CheckUserPassword(9, Constants.MsPassword).ConfigureAwait(true);

            //Bestehender, angemeldeter Benutzer
            if (dc.DeviceAndUserRegisteredLocal)
            {
                if (string.IsNullOrWhiteSpace(args))
                {
                    _cmdHome.IsSelected = true;
                }
                else
                {
                    NavigatorExtensions.BcNavigation(null!)!.ToView("ViewMain", args);
                }
            }
            else
            {
                NavigatorExtensions.BcNavigation(null!)!.ToView("ViewLogin", true, false);
            }
        }

        #region VmCommands für alle Views

        private static VmCommandSelectable _cmdMenuFooter = null!;

        private static VmCommandSelectable _cmdMe = null!;
        private static VmCommandSelectable _cmdHome = null!;

        private static VmCommandSelectable _cmdAdminHome = null!;

        private static VmCommandSelectable _cmdConfigTool = null!;

        private static VmCommandSelectable _cmdSensorStatistics = null!;

        private static bool _showNotConnected;


        public VmCommandSelectable CmdMenuFooter => _cmdMenuFooter;

        public VmCommandSelectable CmdMe => _cmdMe;
        public VmCommandSelectable CmdHome => _cmdHome;

        public VmCommandSelectable CmdAdminHome => _cmdAdminHome;

        public VmCommandSelectable CmdConfigTool => _cmdConfigTool;

        public VmCommandSelectable CmdSensorStatistics => _cmdSensorStatistics;


        /// <summary>
        ///     Projektbeogene, globale VmCommands(Selectable) initialisieren
        /// </summary>
        protected override bool InitializeProjectBaseCommands()
        {
            _cmdMenuFooter = new VmCommandSelectable(ResViewFooterNavigation.Cmd_More, async () =>
            {
                CmdShowMenu.Execute(null!);
                await Task.Delay(700).ConfigureAwait(true);
                _cmdMenuFooter.IsSelected = false;
            }, glyph: "\uE93F");

            //UserApps commands
            _cmdMe = new VmCommandSelectable(ResViewMenu.CmdMe, async () => { Nav.ToView("ViewMe", showMenu: true); }, ResViewMenu.CmdMeToolTip, "\ue965");

            _cmdHome = new VmCommandSelectable(ResViewMenu.CmdHome, () => { Nav.ToView("ViewMain", showMenu: true); }, ResViewMenu.CmdHomeToolTip, "\uE99A");


            //AdminApps commands
            _cmdAdminHome = new VmCommandSelectable(ResViewAdminMain.Title, () => { Nav.ToView("ViewAdminMain", showMenu: true); }, glyph: "\uE92E");

            _cmdConfigTool = new VmCommandSelectable(ResViewConfigurationTool.Title, () => { Nav.ToView("ViewConfigurationTool", showMenu: true); }, glyph: "\uE91E");
            _cmdSensorStatistics = new VmCommandSelectable("Sensor Statistics", () => { Nav.ToView("ViewSensorStatistics", showMenu: true); }, glyph: "\uE903");

            return true;
        }

        #endregion
    }
}