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
using BaseApp.ViewModel.ConfigurationTool.Interfaces;
using Biss.Dc.Client;
using Exchange.Model.ConfigurationTool;

namespace BaseApp.ViewModel.ConfigurationTool
{
    /// <summary>
    ///     ViewModel vom Sensor.
    /// </summary>
    /// <seealso cref="BaseApp.ViewModel.ConfigurationTool.Interfaces.IVmEditable" />
    public class VmSensor : IVmEditable
    {
        /// <summary>
        ///     Aufruf um diesen Objekt vom Eltern zu entfernen.
        /// </summary>
        private readonly Action<VmSensor> _disposeCallBack = null!;

        // Dummy/Clone Constructor
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmSensor" /> class.
        /// </summary>
        public VmSensor()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VmSensor" /> class.
        /// </summary>
        /// <param name="dataPoint">The data point.</param>
        /// <param name="disposeCallBack">The dispose call back.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     dataPoint
        ///     or
        ///     disposeCallBack
        /// </exception>
        public VmSensor(DcListDataPoint<ExSensor> dataPoint, Action<VmSensor> disposeCallBack)
        {
            DataPoint = dataPoint ?? throw new ArgumentNullException(nameof(dataPoint));
            _disposeCallBack = disposeCallBack ?? throw new ArgumentNullException(nameof(disposeCallBack));
        }

        #region Properties

        /// <summary>
        ///     Der unterliegende DatenPunkt.
        /// </summary>
        public DcListDataPoint<ExSensor> DataPoint { get; } = new DcListDataPoint<ExSensor>(new ExSensor());

        /// <summary>
        ///     Name vom Sensor.
        /// </summary>
        public string Name => DataPoint.Data.Name;

        /// <summary>
        ///     Die SensorId des sensors.
        /// </summary>
        public long SensorId => DataPoint.Data.SensorId;

        #endregion

        /// <summary>
        ///     Entfernt diesen Instanz von seinem Eltern.
        ///     Deutlich schneller als manuel zu entfernen, deshalb.
        /// </summary>
        public void Dispose() =>
            _disposeCallBack?.Invoke(this);

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