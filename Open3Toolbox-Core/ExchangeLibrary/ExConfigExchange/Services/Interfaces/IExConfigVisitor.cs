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
using System.Collections;
using ExConfigExchange.Models;
using ExConfigExchange.Models.Interfaces;

namespace ExConfigExchange.Services.Interfaces
{
    /// <summary>
    ///     Dieser Interface ist einen generischen Visitor Interface für <see cref="IExConfigItem" />s.
    ///     Man kann diese für die Rendering oder allgemeine Unterscheidung von <see cref="IExConfigItem" /> implementationen
    ///     verwenden.
    /// </summary>
    /// <typeparam name="TOut">Der generische Rückgabewert.</typeparam>
    public interface IExConfigVisitor<TOut>
    {
        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExStringConfigItem" /> instanz.
        /// </summary>
        /// <param name="exStringConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="string" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExStringConfigItem exStringConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExUrlConfigItem" /> instanz.
        /// </summary>
        /// <param name="exUrlConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="Uri" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExUrlConfigItem exUrlConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExByteConfigItem" /> instanz.
        /// </summary>
        /// <param name="exByteConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="byte" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExByteConfigItem exByteConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExIntConfigItem" /> instanz.
        /// </summary>
        /// <param name="exIntConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="int" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExIntConfigItem exIntConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExLongConfigItem" /> instanz.
        /// </summary>
        /// <param name="exLongConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="long" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExLongConfigItem exLongConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExFloatConfigItem" /> instanz.
        /// </summary>
        /// <param name="exFloatConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="float" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExFloatConfigItem exFloatConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExDoubleConfigItem" /> instanz.
        /// </summary>
        /// <param name="exDoubleConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="double" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExDoubleConfigItem exDoubleConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExBoolConfigItem" /> instanz.
        /// </summary>
        /// <param name="exBoolConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="bool" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExBoolConfigItem exBoolConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExEnumConfigItem" /> instanz.
        /// </summary>
        /// <param name="exEnumConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: enum.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExEnumConfigItem exEnumConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExObjectConfigItem" /> instanz.
        /// </summary>
        /// <param name="exObjectConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="object" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExObjectConfigItem exObjectConfigItem, Func<TOut> optionalCall = null!);

        /// <summary>
        ///     Besucht den spezifizierten <see cref="ExCollectionConfigItem" /> instanz.
        /// </summary>
        /// <param name="exCollectionConfigItem">Der <see cref="IExConfigItem" /> Abbildung vom Type: <see cref="IEnumerable" />.</param>
        /// <param name="optionalCall">
        ///     Mit dieser Func kann man werte vom Eltern zum Kind
        ///     <see cref="IExConfigItem" /> verpasst werden.
        /// </param>
        /// <returns>
        ///     <see cref="TOut" />
        /// </returns>
        TOut Visit(ExCollectionConfigItem exCollectionConfigItem, Func<TOut> optionalCall = null!);
    }
}