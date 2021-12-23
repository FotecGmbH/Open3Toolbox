using System;
using AdminWebApp.Client.Extensions;
using Biss.Dc.Client;
using Exchange.Model.ConfigurationTool.Interfaces;
using Exchange.Resources.ResConfigurationTool;
using ExConfigExchange.Services;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace AdminWebApp.Client.Components
{
    /// <summary>
    /// Backend vom <see cref="ConfigurableEditorComponent{T}"/>, wird für den Typisierung gebraucht.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Microsoft.AspNetCore.Components.ComponentBase" />
    public partial class ConfigurableEditorComponent<T>
        where T : IExConfigurable
    {
        /// <summary>
        /// Der validation visitor.
        /// </summary>
        private ExConfigItemValidationVisitor _validationVisitor = new ExConfigItemValidationVisitor();

        /// <summary>
        /// Der DialogService.
        /// </summary>
        [Inject]
        public DialogService DialogService { get; set; }

        /// <summary>
        /// Der Notification Service.
        /// </summary>
        [Inject]
        public NotificationService NotificationService { get; set; }

        /// <summary>
        /// Die ursprüngliche Name vom <see cref="T"/> Instanz.
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Der Datenpunkt vom <see cref="T"/> Instanz.
        /// </summary>
        [Parameter]
        public DcListDataPoint<T> DataPoint { get; set; }

        /// <summary>
        /// Callback für das Entfernen des Datepunkts.
        /// </summary>
        [Parameter]
        public Action<DcListDataPoint<T>> OnDeleteRequested { get; set; }

        /// <summary>
        /// Callback für das speichern der Änderungen.
        /// </summary>
        [Parameter]
        public Action OnChangesSaved { get; set; }

        /// <summary>
        /// <c>true</c> wenn die Änderungen gespeichert werden können sonst <c>false</c>.
        /// </summary>
        public bool CanSaveChanges { get; set; }

        /// <summary>
        /// Änderungen speichern.
        /// </summary>
        private async void SaveChanges()
        {
            this.DataPoint.EndEdit();
            this.OriginalName = this.DataPoint.Data.Name;
            var result = await this.DataPoint.StoreData().ConfigureAwait(true);
            if (result.DataOk)
                this.NotificationService.NotifyOfSuccess(ResViewConfigurableEditorComponent.NotificationTitle_ChangesSaved, ResViewConfigurableEditorComponent.NotificationMsg_ChangesSaved);
            else
                this.NotificationService.NotifyOfFailure("", result.ServerExceptionText);

            this.DataPoint.BeginEdit();
            this.OnChangesSaved();
        }
    }
}