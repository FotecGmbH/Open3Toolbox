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
using System.Collections.Generic;
using BaseApp.ViewModel.ConfigurationTool.Interfaces;
using Biss.Dc.Client;
using Biss.Interfaces;
using Exchange.Model.ConfigurationTool;

namespace BaseApp.ViewModel.ConfigurationTool
{
    /// <summary>
    ///     ViewModel vom Gateway.
    /// </summary>
    /// <seealso cref="BaseApp.ViewModel.ConfigurationTool.Interfaces.IVmEditable" />
    public class VmGateway : IVmEditable, IBissSerialize
    {
        /// <summary>
        ///     Aufruf um diesen Objekt vom Eltern zu entfernen.
        /// </summary>
        private readonly Action<VmGateway> _disposeCallBack = null!;

        // Dummy/Clone Constructor
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmGateway" /> class.
        /// </summary>
        public VmGateway()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VmGateway" /> class.
        /// </summary>
        /// <param name="dataPoint">The data point.</param>
        /// <param name="disposeCallBack">The dispose call back.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     dataPoint
        ///     or
        ///     disposeCallBack
        /// </exception>
        public VmGateway(DcListDataPoint<ExGateway> dataPoint, Action<VmGateway> disposeCallBack)
        {
            DataPoint = dataPoint ?? throw new ArgumentNullException(nameof(dataPoint));
            _disposeCallBack = disposeCallBack ?? throw new ArgumentNullException(nameof(disposeCallBack));
        }

        #region Properties

        /// <summary>
        ///     Der unterliegende DatenPunkt.
        /// </summary>
        public DcListDataPoint<ExGateway> DataPoint { get; } = new DcListDataPoint<ExGateway>(new ExGateway());

        /// <summary>
        ///     Der Name vom Gateway.
        /// </summary>
        public string Name => DataPoint.Data.Name;

        /// <summary>
        ///     Die Sensoren von diesem Instanz.
        /// </summary>
        public ICollection<VmSensor> Sensors { get; } = new List<VmSensor>();

        #endregion

        /// <summary>
        ///     Convinience Method.
        /// </summary>
        /// <param name="sensor">Der sensor.</param>
        public void AddSensorVm(VmSensor sensor) =>
            Sensors.Add(sensor);

        /// <summary>
        ///     Convinience Method.
        /// </summary>
        /// <param name="sensor">Der sensor.</param>
        public void RemoveSensorVm(VmSensor sensor) =>
            Sensors.Remove(sensor);


        /// <summary>
        ///     Entfernt diesen Instanz von seinem Eltern.
        ///     Deutlich schneller als manuel zu entfernen, deshalb.
        /// </summary>
        public void Dispose() =>
            _disposeCallBack(this);

        /// <summary>
        ///     Override vom <see cref="ToString" />.
        /// </summary>
        /// <returns>
        ///     <see cref="Name" /> von diesem Instanz.
        /// </returns>
        public override string ToString() => Name;

        #region Interface Implementations

        /// <summary>
        ///     Änderungen wegwerfen.
        /// </summary>
        public void UndoChanges() =>
            DataPoint.EndEdit(true);

        /// <summary>
        ///     Edit Mode Beginnen.
        /// </summary>
        public void BeginEdit() =>
            DataPoint.BeginEdit();

        /// <summary>
        ///     Änderungen Speichern.
        /// </summary>
        public void SaveChanges() =>
            DataPoint.EndEdit();

        #endregion
    }
}