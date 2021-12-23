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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Exchange.Model.Statistics;
using Exchange.Resources.ResStatistics;

namespace BaseApp.ViewModel.Statistics
{
    /// <summary>
    ///     ViewModel vom MeasurementHistory (Messungsverlauf).
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    public class VmMeasurementHistory : VmProjectBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmMeasurementHistory" /> class.
        /// </summary>
        public VmMeasurementHistory() : base(ResViewMeasurementHistory.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Messungs Details.
        /// </summary>
        public DcListDataPoint<ExMeasurementDetails> MeasurementDetails { get; private set; } = new DcListDataPoint<ExMeasurementDetails>(new ExMeasurementDetails());

        /// <summary>
        ///     Messungsverlauf.
        /// </summary>
        public IEnumerable<ExMeasurementData> History { get; private set; } = new List<ExMeasurementData>();

        /// <summary>
        ///     Messungsverlauf aktualisieren.
        /// </summary>
        public VmCommand CmdRefreshData { get; private set; } = null!;

        /// <summary>
        ///     Zurück gehen.
        /// </summary>
        public VmCommand CmdGoBack { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args">
        ///     <see cref="DcListDataPoint{ExMeasurementDetails}" />
        /// </param>
        public async override Task OnActivated(object? args = null)
        {
            if (args is DcListDataPoint<ExMeasurementDetails> detailsDataPoint)
            {
                MeasurementDetails = detailsDataPoint;
            }
            else
            {
                Nav.ToView("ViewSensorStatistics");
                return;
            }

            IsBusy = true; // Busy Lock In

            await RefreshHistory().ConfigureAwait(true);

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

                await RefreshHistory().ConfigureAwait(true);

                IsBusy = false; // Busy Lock Out
            });

            CmdGoBack = new VmCommand("", async () =>
            {
                await Dc.DcExMeasurementViews.WaitDataFromServerAsync(startIndex: MeasurementDetails.Index).ConfigureAwait(true);
                var measurementView = Dc.DcExMeasurementViews.FirstOrDefault(mVD => mVD.Index == MeasurementDetails.Index);
                if (measurementView is null)
                {
                    Nav.ToView("ViewSensorStatistics");
                }
                else
                {
                    Nav.ToView("ViewMeasurementDetails", new VmMeasurementView(measurementView));
                }
            });

            base.InitializeCommands();
        }

        /// <summary>
        ///     Aktualisiert den Messungsverlauf.
        /// </summary>
        private async Task RefreshHistory()
        {
            await Dc.DcExMeasurementData.WaitDataFromServerAsync(startIndex: MeasurementDetails.Index).ConfigureAwait(true);
            History = Dc.DcExMeasurementData.Where(mDD => mDD.Data.MeasurementId == MeasurementDetails.Index).Select(mDD => mDD.Data);
        }
    }
}