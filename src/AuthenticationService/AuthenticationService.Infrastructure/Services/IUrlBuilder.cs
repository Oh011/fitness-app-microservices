using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Services
{
    internal interface IUrlBuilder
    {


        public string BuildEmailConfirmationUrl(string rawToken);

        public string BuildPasswordResetUrl(string rawToken);
    }
}
