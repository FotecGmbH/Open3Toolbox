// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:49
// Developer:     Matthias Mandl
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;

namespace OpCodesDllsLibrary
{
    /// <summary>
    ///     Which type the interface is
    /// </summary>
    public enum InterfaceType
    {
        i2CInterface = 0,
        gpioInterface = 1
    }
}