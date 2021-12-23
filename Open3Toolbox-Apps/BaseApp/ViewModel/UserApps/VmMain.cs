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
using System.IO;
using System.Threading.Tasks;
using Biss.Apps.ViewModel;
using Biss.Collections;
using Exchange.Enum;
using Exchange.Helper;
using Exchange.Resources;

// ReSharper disable once CheckNamespace
namespace BaseApp.ViewModel
{
    /// <summary>
    ///     <para>User App Main Viev</para>
    ///     Klasse VmMain. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public class VmMain : VmProjectBase
    {
        /// <summary>
        ///     Umschalten der Work Action
        /// </summary>
        /// <param name="isStart">Ob es gestartet ist</param>
        public delegate void ToggleWorkActionsDelegate(bool isStart);

        /// <summary>
        ///     ViewModel Template
        /// </summary>
        public VmMain() : base(ResViewMain.Title, subTitle: ResViewMain.Subtitle)
        {
            ShowTitle = false;
            IsBusy = false;

            SelectedTimeSpan = TimeSpanPickerItems[1];

            MenuGestureEnabled = false;
        }

        #region Properties

        public static VmMain DesignInstance => new VmMain();

        /// <summary>
        ///     Tag Info
        /// </summary>
        public string DayHeader { get; set; } = "Keine Daten";

        /// <summary>
        ///     Tag Info Details
        /// </summary>
        public string DaySubHeader { get; set; } = "Werden automatisch geladen";

        /// <summary>
        ///     Umschalten der Work Action
        /// </summary>
        public ToggleWorkActionsDelegate ToggleWorkActions { get; set; } = null!;


        /// <summary>
        ///     Bild
        /// </summary>
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public Stream Image => Images.ReadImageAsStream(EnumEmbeddedImage.Logo_png);
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        /// <summary>
        ///     Anzahl der Todos von MS-To-Do
        /// </summary>
        public int NumberOfToDos { get; set; }

        /// <summary>
        ///     Anzahl der Termine von MS-To-Do
        /// </summary>
        public int NumberOfAppointments { get; set; }

        /// <summary>
        ///     Kalender App öffnen Command
        /// </summary>
        public VmCommandSelectable CmdOpenCalendar { get; set; } = null!;

        /// <summary>
        ///     Auswählbare Zeitintervalle für Timer.
        /// </summary>
        public ObservableCollectionFilterable<ItemDisplay<TimeSpan>> TimeSpanPickerItems { get; } = new ObservableCollectionFilterable<ItemDisplay<TimeSpan>>
                                                                                                    {
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromMinutes(5), "5min"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromMinutes(15), "15min"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromMinutes(30), "30min"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromMinutes(45), "45min"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromMinutes(60), "1h"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromMinutes(90), "1h 30min"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromHours(2), "2h"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromHours(3), "3h"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromHours(4), "4h"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromHours(5), "5h"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromHours(6), "6h"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromHours(7), "7h"),
                                                                                                        new ItemDisplay<TimeSpan>(TimeSpan.FromHours(8), "8h")
                                                                                                    };

        /// <summary>
        ///     Die ausgewählte Zeitspanne für den Timer.
        /// </summary>
        public ItemDisplay<TimeSpan>? SelectedTimeSpan { get; set; }

        /// <summary>
        ///     Bemerkung für den Timer -> kommt dann in die Bemerkung der speziellen Zeit
        /// </summary>
        public string TimerAnnotation { get; set; } = string.Empty;

        #endregion

        /// <summary>
        ///     View wurde erzeugt
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public override async Task OnActivated(object? args = null)
        {
#pragma warning disable CA1508 // Avoid dead conditional code
            if (args is string fromContext && !string.IsNullOrWhiteSpace(fromContext))
#pragma warning restore CA1508 // Avoid dead conditional code
            {
                if (fromContext.ToUpperInvariant().Equals("START", StringComparison.Ordinal))
                {
                }

                if (fromContext.ToUpperInvariant().Equals("STOP", StringComparison.Ordinal))
                {
                }
            }
        }

        /// <summary>
        ///     Commands Initialisieren (aufruf im Kostruktor von VmBase)
        /// </summary>
        protected override void InitializeCommands()
        {
            CmdOpenCalendar = new VmCommandSelectable(ResViewMain.Cmd_Appointments, () =>
            {
                OpenWithFeedback(EnumOpenType.Calendar);
                ResetSelectedButton(CmdOpenCalendar);
            });
        }
    }
}