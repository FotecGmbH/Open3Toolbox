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
using System.Linq;
using Biss.Log.Producer;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace BaseApp.View
{
    public partial class ViewMain
    {
        public ViewMain() : this(null)
        {
        }

        public ViewMain(object? args = null) : base(args)
        {
            InitializeComponent();
            if (Navigation.NavigationStack.Count > 0)
            {
                ClearStackPanelAndNavigateFrist(Navigation);
            }

            ViewModel.ToggleWorkActions = ToggleStartStopWorkAction;
        }

        public void ClearStackPanelAndNavigateFrist(INavigation navigation)
        {
            var existingPages = navigation.NavigationStack.ToList();
            foreach (var t in existingPages)
            {
                navigation.RemovePage(t);
            }
        }

        public void ToggleStartStopWorkAction(bool isStart)
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
            {
                try
                {
                    var action = new AppAction("Stop", "Arbeit beenden");

                    if (isStart)
                    {
                        action = new AppAction("Start", "Arbeit beginnen");
                    }

                    MainThread.BeginInvokeOnMainThread(() => AppActions.SetAsync(action));
                }
                catch (Exception e)
                {
                    Logging.Log.LogError($"{e}");
                }
            }
        }
    }
}