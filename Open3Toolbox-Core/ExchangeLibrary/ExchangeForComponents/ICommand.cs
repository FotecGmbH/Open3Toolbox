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

namespace ExchangeLibrary
{
    /// <summary>
    ///     An ICommand can get executed with an object as parameter
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        ///     Execute the ICommand with an object as parameter
        /// </summary>
        /// <param name="parameter">An object as parameter</param>
        public void Execute(object parameter);
    }
}