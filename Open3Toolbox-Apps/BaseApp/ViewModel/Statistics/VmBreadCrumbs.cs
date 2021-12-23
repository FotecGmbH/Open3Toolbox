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

using System;
using System.Collections.Generic;
using BaseApp.ViewModel.Statistics.Interfaces;

namespace BaseApp.ViewModel.Statistics
{
    /// <summary>
    ///     ViewModel für BreadCrumbs.
    /// </summary>
    public static class VmBreadCrumbs
    {
        /// <summary>
        ///     Die bread crumbs
        /// </summary>
        private static readonly Stack<IVmNamed> _breadCrumbs = new Stack<IVmNamed>();

        #region Properties

        /// <summary>
        ///     Die bread crumbs.
        /// </summary>
        public static IEnumerable<IVmNamed> BreadCrumbs => _breadCrumbs;

        #endregion

        /// <summary>
        ///     Pusht den bread crumb in der Form von einen <see cref="IVmNamed" /> Instanz.
        /// </summary>
        /// <param name="crumb">The crumb.</param>
        public static void PushBreadCrumb(IVmNamed crumb) => _breadCrumbs.Push(crumb);

        /// <summary>
        ///     Popt den obersten bread crumb.
        /// </summary>
        public static void PopBreadCrumb() => _breadCrumbs.Pop();

        /// <summary>
        ///     Schaut nach ob der eingepasste crumb ganz oben liegt oder nicht.
        /// </summary>
        /// <param name="crumb">Einen bread crumb in der Form von einen <see cref="IVmNamed" /> Instanz.</param>
        /// <returns>
        ///     <c>true</c> wenn der crumb ganz oben liegt sonst <c>false</c>.
        /// </returns>
        public static bool IsOnTop(IVmNamed crumb) => _breadCrumbs.Count == 0 ? false : _breadCrumbs.Peek() == crumb;
    }
}