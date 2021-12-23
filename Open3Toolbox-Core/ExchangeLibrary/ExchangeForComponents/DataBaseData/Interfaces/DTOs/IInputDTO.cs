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
    ///     Represents an Input-DTO.
    /// </summary>
    public interface IInputDTO
    {
    }

    /// <summary>
    ///     Represents an Input-DTO for the given generic type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IInputDTO<TEntity> : IInputDTO
        where TEntity : class, IEntity
    {
    }
}