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

using System.Linq;
using System.Threading.Tasks;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Exchange.Model.Statistics;
using Exchange.Resources.ResStatistics;

namespace BaseApp.ViewModel.Statistics
{
    /// <summary>
    ///     ViewModel vom ActorDetails.
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    public class VmActorDetails : VmProjectBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmActorDetails" /> class.
        /// </summary>
        public VmActorDetails() : base(ResViewActorDetails.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Ansichtelement.
        /// </summary>
        public VmActorView ViewElement { get; private set; } = new VmActorView(new DcListDataPoint<ExActorView>(new ExActorView()));

        /// <summary>
        ///     Aktor Details.
        /// </summary>
        public DcListDataPoint<ExActorDetails> ActorDetails { get; private set; } = new DcListDataPoint<ExActorDetails>(new ExActorDetails());

        /// <summary>
        ///     Befehl senden.
        /// </summary>
        public VmCommand CmdSendCommand { get; private set; } = null!;

        /// <summary>
        ///     Zurück gehen.
        /// </summary>
        public VmCommand CmdGoBack { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args">
        ///     <see cref="VmActorView" />
        /// </param>
        public async override Task OnActivated(object? args = null)
        {
            if (args is VmActorView viewElement)
            {
                ViewElement = viewElement;
            }
            else
            {
                Nav.ToView("ViewSensorStatistics");
                return;
            }

            IsBusy = true; // Busy Lock In

            await SetActorDetails().ConfigureAwait(true);

            IsBusy = false; // Busy Lock Out

            await base.OnActivated(args).ConfigureAwait(true);
        }


        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdSendCommand = new VmCommand(ResViewActorDetails.Cmd_SendCommand, () =>
            {
                // Zu Implementieren.
            });

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
        ///     Aktor Details Setzen.
        /// </summary>
        private async Task SetActorDetails()
        {
            await Dc.DcExActorDetails.WaitDataFromServerAsync(startIndex: ViewElement.DataPoint.Index).ConfigureAwait(true);
            ActorDetails = Dc.DcExActorDetails.First(mD => mD.Index == ViewElement.DataPoint.Index);
        }
    }
}