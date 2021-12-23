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
    ///     ViewModel vom Messungsansicht.
    /// </summary>
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmFinalView" />
    public class VmMeasurementView : IVmFinalView
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmMeasurementView" /> class.
        /// </summary>
        public VmMeasurementView()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VmMeasurementView" /> class.
        /// </summary>
        /// <param name="dataPoint">The data point.</param>
        /// <exception cref="System.ArgumentNullException">dataPoint</exception>
        public VmMeasurementView(DcListDataPoint<ExMeasurementView> dataPoint)
        {
            DataPoint = dataPoint ?? throw new ArgumentNullException(nameof(dataPoint));
        }

        #region Properties

        /// <summary>
        ///     Der unterliegenden Datenpunkt.
        /// </summary>
        public DcListDataPoint<ExMeasurementView> DataPoint { get; } = new DcListDataPoint<ExMeasurementView>(new ExMeasurementView());

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
        public string Type => ResViewFinalSubViewPage.Type_Measurement;

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
            new VmMeasurementView(new DcListDataPoint<ExMeasurementView>(BissDeserialize.FromJson<ExMeasurementView>(DataPoint.Data.ToJson())));

        #endregion
    }
}