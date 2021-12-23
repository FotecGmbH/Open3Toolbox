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
    ///     Generische Visitor für die Type unterscheidung vom <see cref="IVmFinalView" /> Implementationen.
    /// </summary>
    /// <typeparam name="T">Return <see cref="Type" /> vom <see cref="Visit" /></typeparam>
    public interface IVmFinalViewVisitor<T>
    {
        /// <summary>
        ///     Besucht den logischen Aktoransicht.
        /// </summary>
        /// <param name="vmActorView">Der logischen Aktoransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     <see cref="T" />
        /// </returns>
        T Visit(VmActorView vmActorView, Func<T> optionalCall);

        /// <summary>
        ///     Besucht den logischen Messungsansicht.
        /// </summary>
        /// <param name="vmMeasurementView">Der logischen Messungsansicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     <see cref="T" />
        /// </returns>
        T Visit(VmMeasurementView vmMeasurementView, Func<T> optionalCall);
    }
}