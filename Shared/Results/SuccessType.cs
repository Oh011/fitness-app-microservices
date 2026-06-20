using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authenticore.Shared.Results
{
    public enum SuccessType
    {
        OK,        // default 200
        Created,   // 201
        NoContent  // 204
    }
}
