using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Dtos.Internal
{
    public class RefreshTokenDto
    {

        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
