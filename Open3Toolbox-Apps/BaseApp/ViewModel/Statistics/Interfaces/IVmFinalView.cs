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
    ///     Markiert eine ViewModel als einen FinalView.
    /// </summary>
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmVisitableFinalView" />
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmNamed" />
    /// <seealso
    ///     cref="BaseApp.ViewModel.Statistics.Interfaces.IVmCloneable&lt;BaseApp.ViewModel.Statistics.Interfaces.IVmFinalView&gt;" />
    public interface IVmFinalView : IVmVisitableFinalView, IVmNamed, IVmCloneable<IVmFinalView>
    {
        #region Properties

        /// <summary>
        ///     Der Type (Nicht <see cref="System.Type" />!) vom FinalView.
        /// </summary>
        string Type { get; }

        #endregion
    }
}