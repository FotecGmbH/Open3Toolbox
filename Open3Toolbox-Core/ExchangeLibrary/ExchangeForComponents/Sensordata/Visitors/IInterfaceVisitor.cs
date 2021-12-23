// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using ExchangeLibrary.SensorData.GeneratedModels.Generated;

namespace ExchangeLibrary.SensorData.Visitors
{
    /// <summary>
    ///     The IInterfaceVisitor handles different incomming Interfaces
    /// </summary>
    public interface IInterfaceVisitor
    {
        /// <summary>
        ///     Visits the I2CIntervace
        /// </summary>
        /// <param name="visitable">The visitable Interface</param>
        public void Visit(I2CInterface visitable);

        /// <summary>
        ///     Visits the GPIOInterface
        /// </summary>
        /// <param name="visitable">The visitable Interface</param>
        public void Visit(GpioInterface visitable);
    }
}