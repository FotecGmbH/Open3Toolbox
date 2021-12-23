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
    ///     Markiert eine ViewModel als Named(hat einen Namen).
    /// </summary>
    public interface IVmNamed
    {
        #region Properties

        /// <summary>
        ///     Der Name von diesem Instanz.
        /// </summary>
        string Name { get; set; }

        #endregion
    }
}