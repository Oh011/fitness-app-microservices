using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Errors
{


    public enum ErrorType
    {
        Validation,
        NotFound,
        Conflict,
        Infrastructure,
        Unauthorized,
        Forbidden,
        Internal
    }
}
