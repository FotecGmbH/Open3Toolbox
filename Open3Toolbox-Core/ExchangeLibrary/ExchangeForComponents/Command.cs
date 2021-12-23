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
    using System;

    /// <summary>
    ///     On Execute the Command executes it
    ///     Action with the given parameter
    /// </summary>
    [Serializable]
    public class Command : ICommand
    {
        /// <summary>
        ///     The action to be executed
        /// </summary>
        private readonly Action<object> _action;

        /// <summary>
        ///     Initialize a new instance of a command
        /// </summary>
        /// <param name="action">The action to be executed</param>
        public Command(Action<object> action)
        {
            _action = action;
        }

        #region Interface Implementations

        /// <summary>
        ///     Execute the ICommand with an object as parameter
        /// </summary>
        /// <param name="parameter">An object as parameter</param>
        public void Execute(object parameter)
        {
            _action.Invoke(parameter);
        }

        #endregion
    }
}