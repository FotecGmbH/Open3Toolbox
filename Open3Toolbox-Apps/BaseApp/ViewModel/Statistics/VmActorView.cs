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
using BaseApp.ViewModel.Statistics.Interfaces;
using Biss.Dc.Client;
using Biss.Serialize;
using Exchange.Model.Statistics;
using Exchange.Resources.ResStatistics;

namespace BaseApp.ViewModel.Statistics
{
    /// <summary>
    ///     ViewModel vom Aktoransicht.
    /// </summary>
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmFinalView" />
    public class VmActorView : IVmFinalView
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmActorView" /> class.
        /// </summary>
        public VmActorView()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VmActorView" /> class.
        /// </summary>
        /// <param name="dataPoint">The data point.</param>
        /// <exception cref="System.ArgumentNullException">dataPoint</exception>
        public VmActorView(DcListDataPoint<ExActorView> dataPoint)
        {
            DataPoint = dataPoint ?? throw new ArgumentNullException(nameof(dataPoint));
        }

        #region Properties

        /// <summary>
        ///     Der unterligende Datenpunkt.
        /// </summary>
        public DcListDataPoint<ExActorView> DataPoint { get; } = new DcListDataPoint<ExActorView>(new ExActorView());

        /// <summary>
        ///     Der Name von diesem Instanz.
        /// </summary>
        public string Name
        {
            get => DataPoint.Data.Name;
            set => DataPoint.Data.Name = value;
        }

        /// <summary>
        ///     Der Type (Nicht <see cref="System.Type" />!) vom FinalView.
        /// </summary>
        public string Type => ResViewFinalSubViewPage.Type_Actor;

        #endregion

        #region Interface Implementations

        /// <summary>
        ///     Akzeptiert den <see cref="IVmFinalViewVisitor{T}" /> Implementation.
        /// </summary>
        /// <typeparam name="T">Der return <see cref="Type" />.</typeparam>
        /// <param name="visitor">Der visitor.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     <typeparamref name="T" />
        /// </returns>
        public T Accept<T>(IVmFinalViewVisitor<T> visitor, Func<T> optionalCall = null!) =>
            visitor.Visit(this, optionalCall);

        /// <summary>
        ///     Klont dieser Instanz.
        /// </summary>
        /// <returns>
        ///     Der Klon.
        /// </returns>
        public IVmFinalView Clone() =>
            new VmActorView(new DcListDataPoint<ExActorView>(BissDeserialize.FromJson<ExActorView>(DataPoint.Data.ToJson())));

        #endregion
    }
}