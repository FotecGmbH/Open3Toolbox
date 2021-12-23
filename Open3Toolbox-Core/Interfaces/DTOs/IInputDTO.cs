using Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces.DTOs
{
    /// <summary>
    /// Represents an Input-DTO.
    /// </summary>
    public interface IInputDTO { }

    /// <summary>
    /// Represents an Input-DTO for the given generic type.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public interface IInputDTO<TEntity> : IInputDTO
        where TEntity : class, IEntity
    {
    }
}
