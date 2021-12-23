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

using ExchangeLibrary.DataBaseData.Interfaces.Models;

namespace ExchangeLibrary.DataBaseData.Interfaces.DTOs
{
    /// <summary>
    ///     Represents an Output-DTO.
    /// </summary>
    public interface IOutputDTO
    {
    }

    /// <summary>
    ///     Represents an Output-DTO for the given generic type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IOutputDTO<TEntity> : IOutputDTO
        where TEntity : class, IEntity
    {
    }
}