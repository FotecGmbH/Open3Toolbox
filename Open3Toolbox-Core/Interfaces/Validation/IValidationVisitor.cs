using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Validation
{
    public interface IValidationVisitor
    {
        void Validate<T>(T entity) where T : IValidationVisitable;

        Task ValidateAsync<T>(T entity) where T : IValidationVisitable;
    }
}
