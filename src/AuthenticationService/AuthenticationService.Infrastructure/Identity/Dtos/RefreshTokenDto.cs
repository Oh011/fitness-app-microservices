using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Dtos
{
    internal class RefreshTokenDto
    {

        public string Token { get; set; }
        public DateTime Expiration { get; set; } 
    }
}
