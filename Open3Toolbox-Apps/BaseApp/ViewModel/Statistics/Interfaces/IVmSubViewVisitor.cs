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

using System;

namespace BaseApp.ViewModel.Statistics.Interfaces
{
    /// <summary>
    ///     Generische Visitor für die Type unterscheidung vom <see cref="IVmSubView" /> Implementationen.
    /// </summary>
    /// <typeparam name="T">Return <see cref="Type" /> vom <see cref="Visit" /></typeparam>
    public interface IVmSubViewVisitor<T>
    {
        /// <summary>
        ///     Besucht den logischen Unteransicht.
        /// </summary>
        /// <param name="vmSubView">Der Unteransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     <see cref="T" />
        /// </returns>
        T Visit(VmSubView vmSubView, Func<T> optionalCall = null!);

        /// <summary>
        ///     Besucht den logischen final Unteransicht.
        /// </summary>
        /// <param name="vmFinalSubView">Der final Unteransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     <see cref="T" />
        /// </returns>
        T Visit(VmFinalSubView vmFinalSubView, Func<T> optionalCall = null!);
    }
}