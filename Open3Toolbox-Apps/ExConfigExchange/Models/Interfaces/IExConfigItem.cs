// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Das Forschungsunternehmen der Fachhochschule Wiener Neustadt
// 
// Kontakt biss@fotec.at / www.fotec.at
// 
// Entwickler      Istvan Galfi
// Projekt         Dataskop

using System;
using System.ComponentModel;
using Biss.Interfaces;
using ExConfigExchange.JsonUtils;
using ExConfigExchange.Services.Interfaces;
using Newtonsoft.Json;

namespace ExConfigExchange.Models.Interfaces
{
    /// <summary>
    /// Diese Interface ist für die unterscheidung zwischen <see cref="IExConfigItem"/>s da. <br/>
    /// ### Sollte eine neue Implementierung von diesem Interface zugefügt werden, muss dieses mit hilfe des <see cref="ExJsonInheritanceAttribute"/> hier addiert werden! ###
    /// </summary>
    /// <seealso cref="Biss.Interfaces.IBissSerialize" />
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    [JsonConverter(typeof(ExJsonInheritanceConverter<IExConfigItem>), "type")]
    [ExJsonInheritance("string", typeof(ExStringConfigItem))]
    [ExJsonInheritance("url", typeof(ExUrlConfigItem))]
    [ExJsonInheritance("byte", typeof(ExByteConfigItem))]
    [ExJsonInheritance("int", typeof(ExIntConfigItem))]
    [ExJsonInheritance("long", typeof(ExLongConfigItem))]
    [ExJsonInheritance("float", typeof(ExFloatConfigItem))]
    [ExJsonInheritance("double", typeof(ExDoubleConfigItem))]
    [ExJsonInheritance("bool", typeof(ExBoolConfigItem))]
    [ExJsonInheritance("enum", typeof(ExEnumConfigItem))]
    [ExJsonInheritance("object", typeof(ExObjectConfigItem))]
    [ExJsonInheritance("collection", typeof(ExCollectionConfigItem))]
    public interface IExConfigItem : IBissSerialize, INotifyPropertyChanged
    {
        /// <summary>
        /// Mit Hilfe des Display-Keys sollte die Sprachspezifische (Englisch, Deutsch uws.) Name eines Objekts/Properties geholt werden.
        /// Dieses kann in abgebildeten Klassen mit Hilfe des <see cref="Annotations.DisplayKeyAttribute"/> festgelegt werden.
        /// </summary>
        /// <value>
        /// Der display key.
        /// </value>
        string DisplayKey { get; set; }

        /// <summary>
        /// Sollte einen Property/Field in eine abgebildete Klasse nicht den Benutzer angezeigt werden, kann dieses Property mit Hilfe des <see cref="Annotations.HiddenAttribute"/> auf <c>true</c> gesetzt werden.
        /// </summary>
        /// <value>
        ///   <c>true</c> wenn versteckt; sonst, <c>false</c>.
        /// </value>
        bool Hidden { get; set; }

        /// <summary>
        /// Sollte einen Property/Field in eine abgebildete Klasse den Benutzer nur angezeigt werden aber nicht modifizierbar sein, kann dieses Property mit Hilfe des <see cref="Annotations.ReadOnlyAttribute"/> auf <c>true</c> gesetzt werden.
        /// </summary>
        /// <value>
        ///   <c>true</c> wenn nur lesbar, sonst, <c>false</c>.
        /// </value>
        bool ReadOnly { get; set; }

        /// <summary>
        /// Akzeptiert den generischen visitor.
        /// </summary>
        /// <typeparam name="T">Der Type des Visitors return Wert.</typeparam>
        /// <param name="visitor">Der visitor.</param>
        /// <param name="optionalCall">
        /// Wird für die weiter passen von Information zu den Kindern des Configs verwendet (e.g.: Delete Button bei <see cref="ExCollectionConfigItem"/>).
        /// </param>
        /// <returns>
        /// Einen generischen wert.
        /// </returns>
        T Accept<T>(IExConfigVisitor<T> visitor, Func<T> optionalCall = null);
    }
}
