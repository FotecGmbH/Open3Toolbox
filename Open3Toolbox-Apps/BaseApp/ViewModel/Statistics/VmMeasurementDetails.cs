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
using System.Linq;
using System.Threading.Tasks;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Exchange.Model.Statistics;
using Exchange.Resources.ResStatistics;

namespace BaseApp.ViewModel.Statistics
{
    /// <summary>
    ///     ViewModel vom MeasurementDetails.
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    public class VmMeasurementDetails : VmProjectBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmMeasurementDetails" /> class.
        /// </summary>
        public VmMeasurementDetails() : base(ResViewMeasurementDetails.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Ansichtelement.
        /// </summary>
        public VmMeasurementView ViewElement { get; private set; } = new VmMeasurementView(new DcListDataPoint<ExMeasurementView>(new ExMeasurementView()));

        /// <summary>
        ///     Messung Details.
        /// </summary>
        public DcListDataPoint<ExMeasurementDetails> MeasurementDetails { get; private set; } = new DcListDataPoint<ExMeasurementDetails>(new ExMeasurementDetails());

        /// <summary>
        ///     Daten aktualisieren (<see cref="MeasurementDetails" /> erneut abrufen mit <see cref="SetMeasurementDetails" />).
        /// </summary>
        public VmCommand CmdRefreshData { get; private set; } = null!;

        /// <summary>
        ///     Weiter zum Messungsverlauf.
        /// </summary>
        public VmCommand CmdToHistory { get; private set; } = null!;

        /// <summary>
        ///     Zurück gehen.
        /// </summary>
        /// <value>
        ///     The command go back.
        /// </value>
        public VmCommand CmdGoBack { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args">
        ///     <see cref="VmMeasurementView" />
        /// </param>
        public async override Task OnActivated(object? args = null)
        {
            if (args is VmMeasurementView viewElement)
            {
                ViewElement = viewElement;
            }
            else
            {
                Nav.ToView("ViewSensorStatistics");
                return;
            }

            IsBusy = true; // Busy Lock In

            await SetMeasurementDetails().ConfigureAwait(true);

            IsBusy = false; // Busy Lock Out

            await base.OnActivated(args).ConfigureAwait(true);
        }


        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdRefreshData = new VmCommand(ResViewCommon.Cmd_RefreshData, async () =>
            {
                IsBusy = true; // Busy Lock In

                await SetMeasurementDetails().ConfigureAwait(true);

                IsBusy = false; // Busy Lock Out
            });

            CmdToHistory = new VmCommand(ResViewMeasurementDetails.BtnTitle_History, () => { Nav.ToView("ViewMeasurementHistory", MeasurementDetails); });

            CmdGoBack = new VmCommand("", async () =>
            {
                await Dc.DcExFinalSubViews.WaitDataFromServerAsync(startIndex: ViewElement.DataPoint.Data.FinalSubViewId).ConfigureAwait(true);
                var finalSubView = Dc.DcExFinalSubViews.FirstOrDefault(fSVD => fSVD.Index == ViewElement.DataPoint.Data.FinalSubViewId);
                if (finalSubView is null)
                {
                    Nav.ToView("ViewSensorStatistics");
                }
                else
                {
                    Nav.ToView("ViewFinalSubViewPage", new VmFinalSubView(finalSubView));
                }
            });
        }

        /// <summary>
        ///     Messung Details setzen.
        /// </summary>
        private async Task SetMeasurementDetails()
        {
            await Dc.DcExMeasurementDetails.WaitDataFromServerAsync(startIndex: ViewElement.DataPoint.Index).ConfigureAwait(true);
            MeasurementDetails = Dc.DcExMeasurementDetails.First(mD => mD.Index == ViewElement.DataPoint.Index);
        }
    }
}