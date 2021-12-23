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
using Biss.Apps.Interfaces;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Biss.Dc.Core;
using Exchange.Model.Statistics;
using Exchange.Resources.ResStatistics;

namespace BaseApp.ViewModel.Statistics
{
    /// <summary>
    ///     ViewModel vom SubViewPage
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    /// <seealso
    ///     cref="BaseApp.ViewModel.Statistics.Interfaces.IVmSubViewVisitor&lt;System.Threading.Tasks.Task&lt;System.Boolean&gt;&gt;" />
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmSubViewVisitor&lt;System.Boolean&gt;" />
    public class VmSubViewPage : VmProjectBase, IVmSubViewVisitor<Task<bool>>, IVmSubViewVisitor<bool>
    {
        /// <summary>
        ///     Die bereits entfernte <see cref="IVmSubView" />s.
        /// </summary>
        private readonly ICollection<IVmSubView> _removedSubViews = new List<IVmSubView>();

        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        public VmSubViewPage() : base(ResViewSubViewPage.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Die ursprüngliche Name vom <see cref="SubView" />.
        /// </summary>
        public string OriginalName { get; private set; } = "";

        /// <summary>
        ///     Der Unteransicht (zBs: ein Haus, Lager usw.).
        /// </summary>
        public VmSubView SubView { get; private set; } = new VmSubView(new DcListDataPoint<ExSubView>(new ExSubView()));

        /// <summary>
        ///     <c>true</c> wenn der <see cref="SubView" /> geändert wurde.
        /// </summary>
        public bool SubViewModified { get; set; }

        /// <summary>
        ///     Die Unteransichten (wie <see cref="VmSubView" /> und <see cref="VmFinalSubView" />).
        /// </summary>
        public ICollection<IVmSubView> SubViews { get; private set; } = new List<IVmSubView>();

        /// <summary>
        ///     Unteransicht zufügen.
        /// </summary>
        public VmCommand CmdAddSubView { get; private set; } = null!;

        /// <summary>
        ///     Unteransicht entfernen.
        /// </summary>
        public VmCommand CmdRemoveSubView { get; private set; } = null!;

        /// <summary>
        ///     Änderungen Speichern.
        /// </summary>
        public VmCommand CmdSaveChanges { get; private set; } = null!;

        /// <summary>
        ///     Änderungen Wegwerfen.
        /// </summary>
        public VmCommand CmdUndoChanges { get; private set; } = null!;

        /// <summary>
        ///     Zurück gehen.
        /// </summary>
        public VmCommand CmdGoBack { get; private set; } = null!;

        /// <summary>
        ///     Unteransicht besuchen.
        /// </summary>
        public VmCommand CmdVisitSubView { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args"></param>
        public async override Task OnActivated(object? args = null)
        {
            if (args is VmSubView subView)
            {
                SubView = subView;
            }
            else
            {
                Nav.ToView("ViewSensorStatistics");
                return;
            }

            OriginalName = SubView.Name;
            SubView.DataPoint.BeginEdit();

            IsBusy = true; // Busy Lock In
            await RefreshSubViews().ConfigureAwait(true);
            IsBusy = false; // Busy Lock Out

            await base.OnActivated(args).ConfigureAwait(true);
        }

        /// <summary>
        ///     View wurde inaktiv
        /// </summary>
        /// <param name="view"></param>
        public override async Task OnDisappearing(IView view)
        {
            await UndoChanges().ConfigureAwait(true);

            await base.OnDisappearing(view).ConfigureAwait(true);
        }

        /// <summary>
        ///     Unteransichten aktualisieren.
        /// </summary>
        public async Task RefreshSubViews()
        {
            await Dc.DcExSubViews.WaitDataFromServerAsync(startIndex: SubView.DataPoint.Index).ConfigureAwait(true);
            await Dc.DcExFinalSubViews.WaitDataFromServerAsync(startIndex: 0, filter: SubView.DataPoint.Index.ToString()).ConfigureAwait(true);
            SubViews = new List<IVmSubView>().Concat(Dc.DcExSubViews.Where(sVD => sVD.Data.SubViewId == SubView.DataPoint.Index).Select(sVD => new VmSubView(sVD))) // Where-s are used due to caching.
                .Concat(Dc.DcExFinalSubViews.Where(fsVD => fsVD.Data.SubViewId == SubView.DataPoint.Index).Select(fsVD => new VmFinalSubView(fsVD))).ToList();
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
                                                                        SubViewId = SubView.DataPoint.Index
                                                                    })),
                       new VmFinalSubView(new DcListDataPoint<ExFinalSubView>(new ExFinalSubView
                                                                              {
                                                                                  Name = ResViewCommon.Title_NewFinalSubView,
                                                                                  SubViewId = SubView.DataPoint.Index
                                                                              })),
                   };
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdAddSubView = new VmCommand("", i =>
            {
                if (i is IVmSubView subView)
                {
                    SubViews.Add(subView.Clone());
                    SubViewModified = true;
                }
            });

            CmdRemoveSubView = new VmCommand("", i =>
            {
                if (i is IVmSubView subView)
                {
                    SubViews.Remove(subView);
                    _removedSubViews.Add(subView);
                    SubViewModified = true;
                }
            });

            CmdSaveChanges = new VmCommand(ResViewCommon.Cmd_SaveChanges, async () => { await SaveChanges().ConfigureAwait(true); }, () => SubViewModified);

            CmdUndoChanges = new VmCommand(ResViewCommon.Cmd_DiscardChanges, async () =>
            {
                await UndoChanges().ConfigureAwait(true);
                SubView.DataPoint.BeginEdit();
            }, () => SubViewModified);

            CmdGoBack = new VmCommand("", async () =>
            {
                await UndoChanges().ConfigureAwait(true);

                if (SubView.DataPoint.Data.IsPartOfMainView)
                {
                    Nav.ToView("ViewSensorStatistics");
                }
                else
                {
                    ChangeSubView(new VmSubView(Dc.DcExSubViews.First(sVD => sVD.Index == SubView.DataPoint.Data.SubViewId)));
                }
            });

            CmdVisitSubView = new VmCommand("", i =>
            {
                if (i is IVmSubView subView)
                {
                    subView.Accept(this, () => default(bool));
                }
            }, _ => !SubViewModified);

            base.InitializeCommands();
        }

        /// <summary>
        ///     Versucht aus dem Edit modus zu treten.
        /// </summary>
        /// <param name="undoChanges">Wenn <c>true</c> Änderungen wegwerfen sonst behalten.</param>
        private void TryEndSubViewEdit(bool undoChanges)
        {
            try
            {
                SubView.DataPoint.EndEdit(undoChanges);
            }
            catch (NullReferenceException)
            {
                // Unwichtig.
            }
        }

        /// <summary>
        ///     Änderungen Speichern.
        /// </summary>
        private async Task SaveChanges()
        {
            IsBusy = true; // Busy Lock In

            TryEndSubViewEdit(false);
            SubView.DataPoint.State = EnumDcListElementState.Modified;
            OriginalName = SubView.Name;
            await MakeListChanges().ConfigureAwait(true);

            var sVresult = await SubView.DataPoint.StoreData().ConfigureAwait(true);
            var sVSresult = await Dc.DcExSubViews.StoreAll().ConfigureAwait(true);
            var sFVSresult = await Dc.DcExFinalSubViews.StoreAll().ConfigureAwait(true);

            IsBusy = false; // Busy Lock Out

            await HandleStoreResult(sVresult).ConfigureAwait(true);
            await HandleStoreResult(sVSresult).ConfigureAwait(true);
            await HandleStoreResult(sFVSresult).ConfigureAwait(true);

            SubViewModified = false;
            SubView.DataPoint.BeginEdit();
            _removedSubViews.Clear();
        }

        /// <summary>
        ///     Listen Änderungen durchführen.
        /// </summary>
        private async Task MakeListChanges()
        {
            foreach (var subView in SubViews)
            {
                await subView.Accept(this, () => Task.FromResult(true)).ConfigureAwait(true);
            }

            foreach (var removed in _removedSubViews)
            {
                await removed.Accept(this, () => Task.FromResult(false)).ConfigureAwait(true);
            }
        }

        /// <summary>
        ///     Änderungen Wegwerfen.
        /// </summary>
        private async Task UndoChanges()
        {
            IsBusy = true; // Busy Lock In
            TryEndSubViewEdit(true);
            await UndoListChanges().ConfigureAwait(true);
            IsBusy = false; // Busy Lock Out

            OriginalName = SubView.Name;
            SubViewModified = false;
        }

        /// <summary>
        ///     Listen Änderungen Wegwerfen.
        /// </summary>
        private async Task UndoListChanges()
        {
            _removedSubViews.Clear();
            await RefreshSubViews().ConfigureAwait(true);
        }

        /// <summary>
        ///     Handles the store result.
        /// </summary>
        /// <param name="result">The result.</param>
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

        /// <summary>
        ///     Changes the sub view.
        /// </summary>
        /// <param name="newSubView">The new sub view.</param>
        private async void ChangeSubView(VmSubView newSubView)
        {
            SubView = newSubView;
            OriginalName = SubView.Name;
            SubView.DataPoint.BeginEdit();

            IsBusy = true; // Busy Lock In

            await RefreshSubViews().ConfigureAwait(true);

            IsBusy = false; // Busy Lock Out
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


            var add = await optionalCall.Invoke().ConfigureAwait(true);
            if (add && vmSubView.DataPoint.Index > 0) // Das bedeutet es ist bereits Teil der Liste
            {
                return true;
            }

            if (add)
            {
                Dc.DcExSubViews.Add(vmSubView.DataPoint);
            }
            else
            {
                Dc.DcExSubViews.Remove(vmSubView.DataPoint);
            }

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

            var add = await optionalCall.Invoke().ConfigureAwait(true);
            if (add && vmFinalSubView.DataPoint.Index > 0) // Das bedeutet es ist bereits Teil der Liste
            {
                return true;
            }

            if (add)
            {
                Dc.DcExFinalSubViews.Add(vmFinalSubView.DataPoint);
            }
            else
            {
                Dc.DcExFinalSubViews.Remove(vmFinalSubView.DataPoint);
            }

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

            TryEndSubViewEdit(true);
            ChangeSubView(vmSubView); // Da wir nicht auf eiem anderen View überkommen, müssen nur die angezeigte Daten ausgetauscht werden.
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