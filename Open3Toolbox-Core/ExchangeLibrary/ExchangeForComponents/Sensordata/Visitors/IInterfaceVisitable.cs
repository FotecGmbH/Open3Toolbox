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

namespace ExchangeLibrary.SensorData.Visitors
{
    /// <summary>
    ///     An IInterfaceVisitable implements Accept so a visitor knows which
    ///     Type it currently holds
    /// </summary>
    public interface IInterfaceVisitable
    {
        /// <summary>
        ///     Accepts a visitor so this knows what type the visitable have
        /// </summary>
        /// <param name="visitor">The visitor</param>
        public void Accept(IInterfaceVisitor visitor);
    }
}