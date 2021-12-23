// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       BaseApp
// 
// Released under MIT

using System;
using System.Linq;
using System.Threading.Tasks;
using Biss.Apps.ViewModel;
using Biss.Dc.Client;
using Biss.Dc.Core;
using Exchange.Model.ConfigurationTool;
using Exchange.Resources.ResConfigurationTool;

namespace BaseApp.ViewModel.ConfigurationTool
{
    /// <summary>
    ///     ViewModel vom ProjectPublisher.
    /// </summary>
    /// <seealso cref="BaseApp.VmProjectBase" />
    public class VmProjectPublisher : VmProjectBase
    {
        /// <summary>
        ///     Basis View Model für alle ViewModel
        /// </summary>
        public VmProjectPublisher() : base(ResViewProjectPublisher.Title)
        {
        }

        #region Properties

        /// <summary>
        ///     Der Projekt, der veröffentlicht werden soll.
        /// </summary>
        public VmProject Project { get; private set; } = new VmProject();

        /// <summary>
        ///     <c>true</c> wenn der Projekt bereits publiziert sei.
        /// </summary>
        public bool IsPublished { get; private set; }

        /// <summary>
        ///     Projekt veröffentlichen.
        /// </summary>
        public VmCommand CmdPublishProject { get; private set; } = null!;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args">Die Argumente</param>
        public override async Task OnActivated(object? args = null)
        {
            if (args is VmProject p)
            {
                Project = p;
            }
            else
            {
                await Nav.Back().ConfigureAwait(true);
                return;
            }

            IsBusy = true; // Busy Lock In

            await Dc.DcExPublishedProjects.WaitDataFromServerAsync(startIndex: Project.DataPoint.Index).ConfigureAwait(true);
            if (Dc.DcExPublishedProjects.Any(pP => pP.Index == Project.DataPoint.Index))
            {
                IsPublished = true;
                Project = new VmProject(Dc.DcExPublishedProjects.First(pP => pP.Index == Project.DataPoint.Index));
            }

            IsBusy = false; // Busy Lock Out

            await base.OnActivated(args).ConfigureAwait(true);
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdPublishProject = new VmCommand(ResViewProjectPublisher.Publish, async () =>
            {
                if (!IsPublished)
                {
                    var dP = new DcListDataPoint<ExProject>(Project.DataPoint.Data);
                    Dc.DcExPublishedProjects.Add(dP);
                    dP.Index = Project.DataPoint.Index;
                }
                else
                {
                    Project.DataPoint.State = EnumDcListElementState.Modified;
                }

                IsBusy = true; // Busy Lock In

                var result = await Dc.DcExPublishedProjects.StoreAll().ConfigureAwait(true);

                IsBusy = false; // Busy Lock Out

                if (!result.DataOk)
                {
                    await MsgBox.Show(result.ServerExceptionText).ConfigureAwait(true);
                    if (!IsPublished)
                    {
                        Dc.DcExPublishedProjects.Clear();
                    }

                    return;
                }

                IsPublished = true;
                await MsgBox.Show($"{Project.Name} {ResViewConnectorCommon.Msg_Success} {ResViewProjectPublisher.Published}").ConfigureAwait(true);
                Nav.ToView("ViewConfigurationTool");
            });
            base.InitializeCommands();
        }
    }
}