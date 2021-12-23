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

namespace BaseApp.ViewModel.Statistics
{
    /// <summary>
    ///     ViewModel vom Unteransicht.
    /// </summary>
    /// <seealso cref="BaseApp.ViewModel.Statistics.Interfaces.IVmSubView" />
    public class VmSubView : IVmSubView
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmSubView" /> class.
        /// </summary>
        public VmSubView()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VmSubView" /> class.
        /// </summary>
        /// <param name="dataPoint">The data point.</param>
        /// <exception cref="System.ArgumentNullException">dataPoint</exception>
        public VmSubView(DcListDataPoint<ExSubView> dataPoint)
        {
            DataPoint = dataPoint ?? throw new ArgumentNullException(nameof(dataPoint));
        }

        #region Properties

        /// <summary>
        ///     Der unterligende Datenpunkt.
        /// </summary>
        public DcListDataPoint<ExSubView> DataPoint { get; } = new DcListDataPoint<ExSubView>(new ExSubView());

        /// <summary>
        ///     Der Name von diesem Instanz.
        /// </summary>
        public string Name
        {
            get => DataPoint.Data.Name;
            set => DataPoint.Data.Name = value;
        }

        #endregion

        /// <summary>
        ///     Override vom <see cref="ToString" />.
        /// </summary>
        /// <returns>
        ///     <see cref="Name" /> von diesem Instanz.
        /// </returns>
        public override string ToString() => Name;

        #region Interface Implementations

        /// <summary>
        ///     Akzeptiert den <see cref="IVmSubViewVisitor{T}" /> Implementation.
        /// </summary>
        /// <typeparam name="T">Der return <see cref="Type" />.</typeparam>
        /// <param name="visitor">Der visitor.</param>
        /// <param name="optionalCall">Der optional call.</param>
        /// <returns>
        ///     <typeparamref name="T" />
        /// </returns>
        public T Accept<T>(IVmSubViewVisitor<T> visitor, Func<T> optionalCall = null!) =>
            visitor.Visit(this, optionalCall);

        /// <summary>
        ///     Klont dieser Instanz.
        /// </summary>
        /// <returns>
        ///     Der Klon.
        /// </returns>
        public IVmSubView Clone() =>
            new VmSubView(new DcListDataPoint<ExSubView>(BissDeserialize.FromJson<ExSubView>(DataPoint.Data.ToJson())));

        #endregion
    }
}