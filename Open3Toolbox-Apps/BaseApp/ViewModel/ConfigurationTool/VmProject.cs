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
using Exchange.Model.ConfigurationTool;

namespace BaseApp.ViewModel.ConfigurationTool
{
    /// <summary>
    ///     ViewModel vom Projekt.
    /// </summary>
    /// <seealso cref="BaseApp.ViewModel.ConfigurationTool.Interfaces.IVmEditable" />
    public class VmProject : IVmEditable
    {
        // Dummy/Clone Constructor
        /// <summary>
        ///     Initializes a new instance of the <see cref="VmProject" /> class.
        /// </summary>
        public VmProject()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="VmProject" /> class.
        /// </summary>
        /// <param name="dataPoint">The data point.</param>
        /// <exception cref="System.ArgumentNullException">dataPoint</exception>
        public VmProject(DcListDataPoint<ExProject> dataPoint)
        {
            DataPoint = dataPoint ?? throw new ArgumentNullException(nameof(dataPoint));
        }

        #region Properties

        /// <summary>
        ///     Der unterliegende DatenPunkt.
        /// </summary>
        public DcListDataPoint<ExProject> DataPoint { get; } = new DcListDataPoint<ExProject>(new ExProject());

        /// <summary>
        ///     Name vom Projekt.
        /// </summary>
        public string Name => DataPoint.Data.Name;

        /// <summary>
        ///     Gateways vom Projekt.
        /// </summary>
        public ICollection<VmGateway> Gateways { get; } = new List<VmGateway>();

        #endregion

        /// <summary>
        ///     Convinience Methods.
        /// </summary>
        /// <param name="gateWay">The gate way.</param>
        public void AddGatewayVm(VmGateway gateWay) =>
            Gateways.Add(gateWay);

        /// <summary>
        ///     Convinience Methods.
        /// </summary>
        /// <param name="gateWay">The gate way.</param>
        public void RemoveGatewayVm(VmGateway gateWay) =>
            Gateways.Remove(gateWay);

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