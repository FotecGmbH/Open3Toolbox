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
    ///     Markiert einen ViewModel als Besuchbar vom <see cref="IVmSubViewVisitor{T}" />.
    /// </summary>
    public interface IVmVisitableView
    {
        /// <summary>
        ///     Akzeptiert den <see cref="IVmSubViewVisitor{T}" /> Implementation.
        /// </summary>
        /// <typeparam name="T">Der return <see cref="Type" />.</typeparam>
        /// <param name="visitor">Der visitor.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     <typeparamref name="T" />
        /// </returns>
        T Accept<T>(IVmSubViewVisitor<T> visitor, Func<T> optionalCall = null!);
    }
}