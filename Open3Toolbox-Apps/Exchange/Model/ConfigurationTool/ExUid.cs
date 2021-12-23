// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       29.11.2021 11:01
// Developer:     Istvan Galfi
// Project:       Exchange
// 
// Released under MIT

using System.ComponentModel;
using Biss.Interfaces;

namespace Exchange.Model.ConfigurationTool
{
    /// <summary>
    ///     Datenträger für die UID vom echten Gateway/Sensor.
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissSerialize" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class ExUid : IBissSerialize, INotifyPropertyChanged
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExUid" /> class.
        /// </summary>
        public ExUid()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExUid" /> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public ExUid(string uid)
        {
            Uid = uid;
        }

        #region Properties

        /// <summary>
        ///     UID vom echten Gateway/Sensor.
        /// </summary>
        public string Uid { get; set; } = string.Empty;

        #endregion

        #region Interface Implementations

#pragma warning disable CS0414
        /// <summary>Occurs when a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged = null!;
#pragma warning restore CS0414

        #endregion
    }
}