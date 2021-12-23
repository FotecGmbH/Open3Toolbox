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
    ///     ViewModel vom FinalSubViewPage
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    /// <seealso
    ///     cref="BaseApp.ViewModel.Statistics.Interfaces.IVmFinalViewVisitor&lt;System.Threading.Tasks.Task&lt;System.Boolean&gt;&gt;" />
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmFinalViewVisitor&lt;System.Boolean&gt;" />
    public class VmFinalSubViewPage : VmProjectBase, IVmFinalViewVisitor<Task<bool>>, IVmFinalViewVisitor<bool>
    {
        /// <summary>
        ///     Die bereits entfernte <see cref="IVmFinalView" />s.
        /// </summary>
        private readonly ICollection<IVmFinalView> _removedFinalViews = new List<IVmFinalView>();

        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        public VmFinalSubViewPage() : base(ResViewFinalSubViewPage.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Die ursprüngliche Name vom <see cref="FinalSubView" />.
        /// </summary>
        public string OriginalName { get; private set; } = "";

        /// <summary>
        ///     Der final Unteransicht (zBs: eine Garage, Zimmer usw.).
        /// </summary>
        public VmFinalSubView FinalSubView { get; set; } = new VmFinalSubView(new DcListDataPoint<ExFinalSubView>(new ExFinalSubView()));

        /// <summary>
        ///     <c>true</c> wenn der <see cref="FinalSubView" /> geändert wurde.
        /// </summary>
        public bool FinalSubViewModified { get; set; }

        /// <summary>
        ///     Die Final Ansichten (wie Aktoren und Messungen).
        /// </summary>
        public ICollection<IVmFinalView> FinalViews { get; private set; } = new List<IVmFinalView>();

        /// <summary>
        ///     Final Ansichten die zum <see cref="FinalSubView" /> zufügen kann.
        /// </summary>
        public ICollection<IVmFinalView> FinalViewOptions { get; private set; } = new List<IVmFinalView>();

        /// <summary>
        ///     Final Ansicht zufügen.
        /// </summary>
        public VmCommand CmdAddFinalView { get; private set; } = null!;

        /// <summary>
        ///     Final Ansicht entfernen.
        /// </summary>
        public VmCommand CmdRemoveFinalView { get; private set; } = null!;

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
        ///     Final Ansicht besuchen.
        /// </summary>
        public VmCommand CmdVisitFinalView { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args">
        ///     <see cref="VmFinalSubView" />
        /// </param>
        public async override Task OnActivated(object? args = null)
        {
            if (args is VmFinalSubView finalSubView)
            {
                FinalSubView = finalSubView;
            }
            else
            {
                Nav.ToView("ViewSensorStatistics");
                return;
            }

            OriginalName = FinalSubView.Name;
            FinalSubView.DataPoint.BeginEdit();

            IsBusy = true; // Busy Lock In
            await RefreshFinalViews().ConfigureAwait(true);
            await RefreshFinalViewOptions().ConfigureAwait(true);
            IsBusy = false; // Busy Lock Out

            await base.OnActivated(args).ConfigureAwait(true);
        }

        /// <summary>
        ///     View wurde inaktiv
        /// </summary>
        /// <param name="view">Die View</param>
        public override async Task OnDisappearing(IView view)
        {
            await UndoChanges().ConfigureAwait(true);

            await base.OnDisappearing(view).ConfigureAwait(true);
        }

        /// <summary>
        ///     Final Ansichten aktualisieren.
        /// </summary>
        public async Task RefreshFinalViews()
        {
            await Dc.DcExActorViews.WaitDataFromServerAsync(startIndex: 0, filter: FinalSubView.DataPoint.Index.ToString()).ConfigureAwait(true);
            await Dc.DcExMeasurementViews.WaitDataFromServerAsync(startIndex: 0, filter: FinalSubView.DataPoint.Index.ToString()).ConfigureAwait(true);
            FinalViews = new List<IVmFinalView>().Concat(Dc.DcExActorViews.Where(sVD => sVD.Data.FinalSubViewId == FinalSubView.DataPoint.Index).Select(sVD => new VmActorView(sVD)))
                .Concat(Dc.DcExMeasurementViews.Where(fsVD => fsVD.Data.FinalSubViewId == FinalSubView.DataPoint.Index).Select(fsVD => new VmMeasurementView(fsVD))).ToList();
        }

        /// <summary>
        ///     Final Ansicht Optionen aktualisieren.
        /// </summary>
        public async Task RefreshFinalViewOptions()
        {
            await Dc.DcExMeasurementViews.WaitDataFromServerAsync(startIndex: 0, filter: $"{nameof(ExMeasurementView.FinalSubViewId)}==null").ConfigureAwait(true);
            await Dc.DcExActorViews.WaitDataFromServerAsync(startIndex: 0, filter: $"{nameof(ExActorView.FinalSubViewId)}==null").ConfigureAwait(true);
            FinalViewOptions = new List<IVmFinalView>().Concat(Dc.DcExActorViews.Where(sVD => sVD.Data.FinalSubViewId == null).Select(sVD => new VmActorView(sVD)))
                .Concat(Dc.DcExMeasurementViews.Where(fsVD => fsVD.Data.FinalSubViewId == null).Select(fsVD => new VmMeasurementView(fsVD))).ToList();
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdAddFinalView = new VmCommand("", i =>
            {
                if (i is IVmFinalView subView)
                {
                    FinalViewOptions.Remove(subView);
                    FinalViews.Add(subView);
                    _removedFinalViews.Remove(subView);
                    FinalSubViewModified = true;
                    CmdAddFinalView.CanExecuteProperty = FinalViewOptions.Count != 0;
                }
            }, _ => FinalViewOptions.Count != 0);

            CmdRemoveFinalView = new VmCommand("", i =>
            {
                if (i is IVmFinalView subView)
                {
                    FinalViewOptions.Add(subView);
                    FinalViews.Remove(subView);
                    _removedFinalViews.Add(subView);
                    FinalSubViewModified = true;
                    CmdAddFinalView.CanExecuteProperty = FinalViewOptions.Count != 0;
                }
            });

            CmdSaveChanges = new VmCommand(ResViewCommon.Cmd_SaveChanges, async () => { await SaveChanges().ConfigureAwait(true); }, () => FinalSubViewModified);

            CmdUndoChanges = new VmCommand(ResViewCommon.Cmd_DiscardChanges, async () =>
            {
                await UndoChanges().ConfigureAwait(true);
                FinalSubView.DataPoint.BeginEdit();
            }, () => FinalSubViewModified);

            CmdGoBack = new VmCommand("", async () =>
            {
                await UndoChanges().ConfigureAwait(true);

                if (FinalSubView.DataPoint.Data.IsPartOfMainView)
                {
                    Nav.ToView("ViewSensorStatistics");
                }
                else
                {
                    Nav.ToView("ViewSubViewPage", new VmSubView(Dc.DcExSubViews.First(sVD => sVD.Index == FinalSubView.DataPoint.Data.SubViewId)));
                }
            });

            CmdVisitFinalView = new VmCommand("", i =>
            {
                if (i is IVmFinalView subView)
                {
                    subView.Accept(this, () => default(bool));
                }
            }, _ => !FinalSubViewModified);

            base.InitializeCommands();
        }

        /// <summary>
        ///     Versucht aus dem Edit modus zu treten.
        /// </summary>
        /// <param name="undoChanges">Wenn <c>true</c> Änderungen wegwerfen sonst behalten.</param>
        private void TryEndFinalSubViewEdit(bool undoChanges)
        {
            try
            {
                FinalSubView.DataPoint.EndEdit(undoChanges);
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

            TryEndFinalSubViewEdit(false);
            FinalSubView.DataPoint.State = EnumDcListElementState.Modified;
            OriginalName = FinalSubView.Name;
            await MakeListChanges().ConfigureAwait(true);

            var sFVresult = await FinalSubView.DataPoint.StoreData().ConfigureAwait(true);
            var aVSresult = await Dc.DcExActorViews.StoreAll().ConfigureAwait(true);
            var mVSresult = await Dc.DcExMeasurementViews.StoreAll().ConfigureAwait(true);

            await RefreshFinalViewOptions().ConfigureAwait(true);

            IsBusy = false; // Busy Lock Out

            await HandleStoreResult(sFVresult).ConfigureAwait(true);
            await HandleStoreResult(aVSresult).ConfigureAwait(true);
            await HandleStoreResult(mVSresult).ConfigureAwait(true);

            FinalSubViewModified = false;
            FinalSubView.DataPoint.BeginEdit();
            _removedFinalViews.Clear();
        }

        /// <summary>
        ///     Listen Änderungen durchführen.
        /// </summary>
        private async Task MakeListChanges()
        {
            foreach (var finalView in FinalViews)
            {
                await finalView.Accept(this, () => Task.FromResult(true)).ConfigureAwait(true);
            }

            foreach (var removed in _removedFinalViews)
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

            TryEndFinalSubViewEdit(true);
            await UndoListChanges().ConfigureAwait(true);
            await RefreshFinalViewOptions().ConfigureAwait(true);

            IsBusy = false; // Busy Lock Out

            OriginalName = FinalSubView.Name;
            FinalSubViewModified = false;
        }

        /// <summary>
        ///     Listen Änderungen Wegwerfen.
        /// </summary>
        private async Task UndoListChanges()
        {
            _removedFinalViews.Clear();
            await RefreshFinalViews().ConfigureAwait(true);
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
        ///     Besucht den logischen Aktoransicht.
        /// </summary>
        /// <param name="vmActorView">Der logischen Aktoransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     Immer <c>true</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     vmActorView
        ///     or
        ///     optionalCall
        /// </exception>
        public async Task<bool> Visit(VmActorView vmActorView, Func<Task<bool>> optionalCall = null!)
        {
            if (vmActorView is null)
            {
                throw new ArgumentNullException(nameof(vmActorView));
            }

            if (optionalCall is null)
            {
                throw new ArgumentNullException(nameof(optionalCall));
            }

            var add = await optionalCall.Invoke().ConfigureAwait(true);
            if (add)
            {
                vmActorView.DataPoint.Data.FinalSubViewId = FinalSubView.DataPoint.Index;
            }
            else
            {
                vmActorView.DataPoint.Data.FinalSubViewId = null;
            }

            vmActorView.DataPoint.State = EnumDcListElementState.Modified;
            return true;
        }

        /// <summary>
        ///     Besucht den logischen Messungsansicht.
        /// </summary>
        /// <param name="vmMeasurementView">Der logischen Messungsansicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     Immer <c>true</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///     vmMeasurementView
        ///     or
        ///     optionalCall
        /// </exception>
        public async Task<bool> Visit(VmMeasurementView vmMeasurementView, Func<Task<bool>> optionalCall = null!)
        {
            if (vmMeasurementView is null)
            {
                throw new ArgumentNullException(nameof(vmMeasurementView));
            }

            if (optionalCall is null)
            {
                throw new ArgumentNullException(nameof(optionalCall));
            }

            var add = await optionalCall.Invoke().ConfigureAwait(true);
            if (add)
            {
                vmMeasurementView.DataPoint.Data.FinalSubViewId = FinalSubView.DataPoint.Index;
            }
            else
            {
                vmMeasurementView.DataPoint.Data.FinalSubViewId = null;
            }

            vmMeasurementView.DataPoint.State = EnumDcListElementState.Modified;
            return true;
        }

        #endregion

        #region Actor- und MeasurementView Besuch Operation Behandlung

        /// <summary>
        ///     Besucht den logischen Aktoransicht.
        /// </summary>
        /// <param name="vmActorView">Der logischen Aktoransicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     Immer <c>true</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">vmActorView</exception>
        public bool Visit(VmActorView vmActorView, Func<bool> optionalCall = null!)
        {
            if (vmActorView is null)
            {
                throw new ArgumentNullException(nameof(vmActorView));
            }

            Nav.ToView("ViewActorDetails", vmActorView);
            return true;
        }

        /// <summary>
        ///     Besucht den logischen Messungsansicht.
        /// </summary>
        /// <param name="vmMeasurementView">Der logischen Messungsansicht.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     Immer <c>true</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">vmMeasurementView</exception>
        public bool Visit(VmMeasurementView vmMeasurementView, Func<bool> optionalCall = null!)
        {
            if (vmMeasurementView is null)
            {
                throw new ArgumentNullException(nameof(vmMeasurementView));
            }

            Nav.ToView("ViewMeasurementDetails", vmMeasurementView);
            return true;
        }

        #endregion
    }
}