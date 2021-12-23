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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseApp.ViewModel.Statistics.Interfaces;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Biss.Dc.Core;
using Exchange.Model.Statistics;
using Exchange.Resources.ResStatistics;

namespace BaseApp.ViewModel.Statistics
{
    /// <summary>
    ///     ViewModel vom SensorStatistics.
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    /// <seealso
    ///     cref="BaseApp.ViewModel.Statistics.Interfaces.IVmSubViewVisitor&lt;System.Threading.Tasks.Task&lt;System.Boolean&gt;&gt;" />
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmSubViewVisitor&lt;System.Boolean&gt;" />
    public class VmSensorStatistics : VmProjectBase, IVmSubViewVisitor<Task<bool>>, IVmSubViewVisitor<bool>
    {
        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        public VmSensorStatistics() : base(ResViewSensorStatistics.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Die Unteransichten.
        /// </summary>
        public ICollection<IVmSubView> SubViews { get; private set; } = new List<IVmSubView>();

        /// <summary>
        ///     <see cref="IVmSubView" /> Instanz zufügen.
        /// </summary>
        public VmCommand CmdAddSubView { get; private set; } = null!;

        /// <summary>
        ///     <see cref="IVmSubView" /> Instanz entfernen.
        /// </summary>
        public VmCommand CmdRemoveSubView { get; private set; } = null!;

        /// <summary>
        ///     <see cref="IVmSubView" /> Instanz besuchen.
        /// </summary>
        public VmCommand CmdVisitSubView { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args">Die Argumente</param>
        public async override Task OnActivated(object? args = null)
        {
            IsBusy = true; // Busy Lock In
            await RefreshSubViews().ConfigureAwait(true);
            IsBusy = false; // Busy Lock Out

            await base.OnActivated(args).ConfigureAwait(true);
        }

        /// <summary>
        ///     <see cref="SubViews" /> aktualisieren.
        /// </summary>
        public async Task RefreshSubViews()
        {
            await Dc.DcExSubViews.WaitDataFromServerAsync(startIndex: 0).ConfigureAwait(true);
            await Dc.DcExFinalSubViews.WaitDataFromServerAsync(startIndex: 0).ConfigureAwait(true);
            SubViews = new List<IVmSubView>().Concat(Dc.DcExSubViews.Where(sVD => sVD.Data.IsPartOfMainView).Select(sVD => new VmSubView(sVD))) // Where-s are used due to caching.
                .Concat(Dc.DcExFinalSubViews.Where(fsVD => fsVD.Data.IsPartOfMainView).Select(fsVD => new VmFinalSubView(fsVD))).ToList();
        }

        /// <summary>
        ///     Holt die mögliche <see cref="IVmSubView" /> Implementationen.
        /// </summary>
        /// <returns>
        ///     Einen <see cref="IEnumerable{IVmSubView}" /> mit dem <see cref="IVmSubView" /> Implementationen.
        /// </returns>
        public IEnumerable<IVmSubView> GetSubViewOptions()
        {
            return new List<IVmSubView>
                   {
                       new VmSubView(new DcListDataPoint<ExSubView>(new ExSubView
                                                                    {
                                                                        Name = ResViewCommon.Title_NewSubView,
                                                                        IsPartOfMainView = true
                                                                    })),
                       new VmFinalSubView(new DcListDataPoint<ExFinalSubView>(new ExFinalSubView
                                                                              {
                                                                                  Name = ResViewCommon.Title_NewFinalSubView,
                                                                                  IsPartOfMainView = true
                                                                              })),
                   };
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdAddSubView = new VmCommand("", async i =>
            {
                if (i is IVmSubView subView)
                {
                    await subView.Clone().Accept(this, () => Task.FromResult(true)).ConfigureAwait(true);
                }
            });

            CmdRemoveSubView = new VmCommand("", async i =>
            {
                if (i is IVmSubView subView)
                {
                    await subView.Accept(this, () => Task.FromResult(false)).ConfigureAwait(true);
                }
            });

            CmdVisitSubView = new VmCommand("", i =>
            {
                if (i is IVmSubView subView)
                {
                    subView.Accept(this, () => default(bool));
                }
            });

            base.InitializeCommands();
        }

        /// <summary>
        ///     Behandelt das Ergebnis des <see cref="DcDataList{T}.StoreAll" /> Operation.
        /// </summary>
        /// <param name="result">Der Ergebnis.</param>
        /// <exception cref="System.ArgumentNullException">result</exception>
        private async Task HandleStoreResult(DcStoreResult result)
        {
            if (result is null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            switch (result.ErrorType)
            {
                case EnumServerError.Connection:
                case EnumServerError.ServerException:
                    IsBusy = false; // Busy Lock Out, egal was, es darf nicht busy sein.
                    await MsgBox.Show(result.ServerExceptionText).ConfigureAwait(true);
                    break;
                case EnumServerError.None:
                    break;
            }
        }

        #region Add/Remove Operation Behandlung

        /// <summary>
        ///     Besucht den logischen Unteransicht.
        /// </summary>
        /// <param name="vmSubView">Der Unteransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     Immer <c>true</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     vmSubView
        ///     or
        ///     optionalCall
        /// </exception>
        public async Task<bool> Visit(VmSubView vmSubView, Func<Task<bool>> optionalCall = null!)
        {
            if (vmSubView is null)
            {
                throw new ArgumentNullException(nameof(vmSubView));
            }

            if (optionalCall is null)
            {
                throw new ArgumentNullException(nameof(optionalCall));
            }

            IsBusy = true; // Busy Lock In
            var add = await optionalCall.Invoke().ConfigureAwait(true);
            if (add)
            {
                Dc.DcExSubViews.Add(vmSubView.DataPoint);
            }
            else
            {
                Dc.DcExSubViews.Remove(vmSubView.DataPoint);
            }

            var storeResult = await Dc.DcExSubViews.StoreAll().ConfigureAwait(true);
            await HandleStoreResult(storeResult).ConfigureAwait(true);

            if (add)
            {
                SubViews.Add(vmSubView);
            }
            else
            {
                SubViews.Remove(vmSubView);
            }

            IsBusy = false; // Busy Lock Out

            return true;
        }

        /// <summary>
        ///     Besucht den logischen final Unteransicht.
        /// </summary>
        /// <param name="vmFinalSubView">Der final Unteransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     Immer <c>true</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     vmFinalSubView
        ///     or
        ///     optionalCall
        /// </exception>
        public async Task<bool> Visit(VmFinalSubView vmFinalSubView, Func<Task<bool>> optionalCall = null!)
        {
            if (vmFinalSubView is null)
            {
                throw new ArgumentNullException(nameof(vmFinalSubView));
            }

            if (optionalCall is null)
            {
                throw new ArgumentNullException(nameof(optionalCall));
            }

            IsBusy = true; // Busy Lock In
            var add = await optionalCall.Invoke().ConfigureAwait(true);
            if (add)
            {
                Dc.DcExFinalSubViews.Add(vmFinalSubView.DataPoint);
            }
            else
            {
                Dc.DcExFinalSubViews.Remove(vmFinalSubView.DataPoint);
            }

            var storeResult = await Dc.DcExFinalSubViews.StoreAll().ConfigureAwait(true);
            await HandleStoreResult(storeResult).ConfigureAwait(true);

            if (add)
            {
                SubViews.Add(vmFinalSubView);
            }
            else
            {
                SubViews.Remove(vmFinalSubView);
            }

            IsBusy = false; // Busy Lock Out

            return true;
        }

        #endregion

        #region Final- und SubView Besuch Operation Behandlung

        /// <summary>
        ///     Besucht den logischen Unteransicht.
        /// </summary>
        /// <param name="vmSubView">Der Unteransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     Immer <c>true</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">vmSubView</exception>
        public bool Visit(VmSubView vmSubView, Func<bool> optionalCall = null!)
        {
            if (vmSubView is null)
            {
                throw new ArgumentNullException(nameof(vmSubView));
            }

            Nav.ToView("ViewSubViewPage", vmSubView);
            return true;
        }

        /// <summary>
        ///     Besucht den logischen final Unteransicht.
        /// </summary>
        /// <param name="vmFinalSubView">Der final Unteransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     Immer <c>true</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">vmFinalSubView</exception>
        public bool Visit(VmFinalSubView vmFinalSubView, Func<bool> optionalCall = null!)
        {
            if (vmFinalSubView is null)
            {
                throw new ArgumentNullException(nameof(vmFinalSubView));
            }

            Nav.ToView("ViewFinalSubViewPage", vmFinalSubView);
            return true;
        }

        #endregion
    }
}