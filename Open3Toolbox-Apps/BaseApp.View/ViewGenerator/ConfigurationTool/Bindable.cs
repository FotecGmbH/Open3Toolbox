// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       BaseApp.View
// 
// Released under MIT

using System;
using System.ComponentModel;

namespace BaseApp.ViewGenerator.ConfigurationTool
{
    /// <summary>
    ///     Diese Klasse dient als einen Proxy für Programmatische Binding.
    /// </summary>
    /// <typeparam name="T"><see cref="Type" /> vom gebundenen Field/Property/Klasse.</typeparam>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class Bindable<T> : INotifyPropertyChanged
    {
        /// <summary>
        ///     Der getter.
        /// </summary>
        private readonly Func<T> _getter;

        /// <summary>
        ///     Der setter.
        /// </summary>
        private readonly Action<T> _setter;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Bindable{T}" /> class.
        /// </summary>
        /// <param name="setter">The setter.</param>
        /// <param name="getter">The getter.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     setter
        ///     or
        ///     getter
        /// </exception>
        public Bindable(Action<T> setter, Func<T> getter)
        {
            _setter = setter ?? throw new ArgumentNullException(nameof(setter));
            _getter = getter ?? throw new ArgumentNullException(nameof(getter));
        }

        #region Properties

        /// <summary>
        ///     Der Proxy.
        /// </summary>
        public T Proxy
        {
            get => _getter();
            set => _setter(value);
        }

        #endregion

        /// <summary>
        ///     Erlaubt das manuelle Aufrufen vom <see cref="PropertyChanged" />.
        /// </summary>
        public void ProxyChanged() =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Proxy)));

        #region Interface Implementations

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}