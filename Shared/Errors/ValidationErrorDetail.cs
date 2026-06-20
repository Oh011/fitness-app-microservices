using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Authenticore.Shared.Errors
{




    public class ValidationErrorDetail
    {
        public string Message { get; set; }

        public ValidationErrorDetail(string message)
        {
            Message = message;
        }
    }
}
