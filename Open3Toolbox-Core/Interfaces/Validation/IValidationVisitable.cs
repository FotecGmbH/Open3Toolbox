using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Validation
{
    public interface IValidationVisitable
    {
        void Accept(IValidationVisitor validator);

        Task AcceptAsync(IValidationVisitor validator);
    }
}
