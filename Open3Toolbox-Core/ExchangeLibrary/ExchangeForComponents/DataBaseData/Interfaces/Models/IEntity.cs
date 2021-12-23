// (C) 2021 FOTEC Forschungs- und Technologietransfer GmbH
// Research Subsidiary of FH Wiener Neustadt
// 
// Contact biss@fotec.at / www.fotec.at
// 
// Created:       23.11.2021 10:48
// Developer:     Istvan Galfi
// Project:       ExchangeLibrary
// 
// Released under MIT

namespace ExchangeLibrary.DataBaseData.Interfaces.Models
{
    /// <summary>
    ///     An entity with an id
    /// </summary>
    public interface IEntity
    {
        #region Properties

        /// <summary>
        ///     The id of the entity
        /// </summary>
        long Id { get; set; }

        #endregion
    }
}