// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       30.08.2021 15:55
// Developer:     Istvan Galfi
// Project:       BaseApp
// 
// Released under MIT

namespace BaseApp.ViewModel.Statistics.Interfaces
{
    /// <summary>
    ///     Markiert einen ViewModel as Klonbar.
    /// </summary>
    /// <typeparam name="TClone">Der <see cref="System.Type" /> vom Klone.</typeparam>
    public interface IVmCloneable<TClone>
    {
        /// <summary>
        ///     Klont dieser Instanz.
        /// </summary>
        /// <returns>Der Klon.</returns>
        TClone Clone();
    }
}