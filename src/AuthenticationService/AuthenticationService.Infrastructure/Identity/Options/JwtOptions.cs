using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Options
{
    public class JwtOptions
    {

        public string Issuer { get; set; }
        public string Audiance { get; set; }
        public string SecretKey { get; set; }
        public int ExpirationInHours { get; set; }

    }
}
