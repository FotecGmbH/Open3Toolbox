using Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interfaces.DTOs
{
    public interface IOutputDTO { }

    public interface IOutputDTO<TEntity> : IOutputDTO
        where TEntity : class, IEntity
    {
    }
}
