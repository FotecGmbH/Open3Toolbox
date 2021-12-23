// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       16.08.2021 08:24
// Developer:     Istvan Galfi
// Project:       BaseApp
// 
// Released under MIT

namespace BaseApp.ViewModel.Statistics.Interfaces
{
    /// <summary>
    ///     Markiert eine ViewModel als einen Unteransicht.
    /// </summary>
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmVisitableView" />
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmNamed" />
    /// <seealso
    ///     cref="BaseApp.ViewModel.Statistics.Interfaces.IVmCloneable{BaseApp.ViewModel.Statistics.Interfaces.IVmSubView}" />
    public interface IVmSubView : IVmVisitableView, IVmNamed, IVmCloneable<IVmSubView>
    {
    }
}