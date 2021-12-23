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

using Biss.Apps.Interfaces;
using Biss.Apps.XF.Navigation.Base;
using Xamarin.Forms.Xaml;

namespace BaseApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewMenu : BaseMenu, IMasterPage
    {
        public ViewMenu()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }

        #region Properties

        /// <summary>
        ///     ViewModel
        /// </summary>
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public VmProjectBase ViewModel => VmProjectBase.GetVmBaseStatic();
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        #endregion

        public override BaseMenu GetNewInstance()
        {
            return new ViewMenu();
        }
    }
}