// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:50
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExConfigExchange.Annotations;
using ExConfigExchange.Models.Interfaces;
using ExConfigExchange.Services.Interfaces;

namespace ExConfigExchange.Models
{
    /// <summary>
    ///     Diese Klasse representiert einen <see cref="int" /> Property/Field von einen abgebildeten Type.
    /// </summary>
    /// <seealso cref="ExConfigExchange.Models.Interfaces.IExConfigItem" />
    public class ExIntConfigItem : IExConfigItem
    {
        #region Properties

        /// <summary>
        ///     Der Wert des abgebildeten Property/Field.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        ///     Wenn <see cref="null" /> <see cref="Value" /> ist immer valid.
        ///     Der Validen berreich in dem der Wert liegen kann mit inklusiven <see cref="ExRange.Min" /> und
        ///     <see cref="ExRange.Max" />. <br />
        ///     Um <see cref="Value" /> als valid erkannt zu werden, muss: <see cref="Value" /> % <see cref="ExRange.Step" /> == 0
        ///     => <see cref="true" /> sein.
        /// </summary>
        public ExRange ValidRange { get; set; }

        /// <summary>
        ///     Wird für die Validierung im View verwendet.
        /// </summary>
        /// <value>
        ///     <c>true</c> wenn valid; sonst, <c>false</c>.
        /// </value>
        public bool Valid { get; set; }

        /// <summary>
        ///     Mit Hilfe des Display-Keys sollte die Sprachspezifische (Englisch, Deutsch uws.) Name eines Objekts/Properties
        ///     geholt werden.
        ///     Dieses kann in abgebildeten Klassen mit Hilfe des <see cref="Annotations.DisplayKeyAttribute" /> festgelegt werden.
        /// </summary>
        /// <value>
        ///     Der display key.
        /// </value>
        public string DisplayKey { get; set; }

        /// <summary>
        ///     Sollte einen Property/Field in eine abgebildete Klasse nicht den Benutzer angezeigt werden, kann dieses Property
        ///     mit Hilfe des <see cref="Annotations.HiddenAttribute" /> auf <c>true</c> gesetzt werden.
        /// </summary>
        /// <value>
        ///     <c>true</c> wenn versteckt; sonst, <c>false</c>.
        /// </value>
        public bool Hidden { get; set; }

        /// <summary>
        ///     Sollte einen Property/Field in eine abgebildete Klasse den Benutzer nur angezeigt werden aber nicht modifizierbar
        ///     sein, kann dieses Property mit Hilfe des <see cref="Annotations.ReadOnlyAttribute" /> auf <c>true</c> gesetzt
        ///     werden.
        /// </summary>
        /// <value>
        ///     <c>true</c> wenn nur lesbar, sonst, <c>false</c>.
        /// </value>
        public bool ReadOnly { get; set; }

        #endregion

        /// <summary>
        ///     Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Interface Implementations

        /// <summary>Akzeptiert den generischen visitor.</summary>
        /// <typeparam name="T">Der Type des Visitors return Wert.</typeparam>
        /// <param name="visitor">Der visitor.</param>
        /// <param name="optionalCall">
        ///     Wird für die weiter passen von Information zu den Kindern des Configs verwendet (e.g.:
        ///     Delete Button bei <see cref="ExCollectionConfigItem" />).
        /// </param>
        /// <returns>Einen generischen wert.</returns>
        public T Accept<T>(IExConfigVisitor<T> visitor, Func<T> optionalCall = null) => visitor.Visit(this, optionalCall);

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}