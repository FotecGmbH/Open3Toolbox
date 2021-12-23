// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       20.07.2021 11:04
// Developer:     Istvan Galfi
// Project:       BaseApp
// 
// Released under MIT

using System;
using Exchange.Enum;

namespace BaseApp
{
    /// <summary>
    ///     <para>DESCRIPTION</para>
    ///     Klasse IOpenHelper. (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
    /// </summary>
    public interface IOpenHelper
    {
        /// <summary>
        ///     Externe App bzw. Settings öffnen.
        /// </summary>
        /// <param name="type">True wenn erfolgreich</param>
        bool OpenExternal(EnumOpenType type);
    }
}