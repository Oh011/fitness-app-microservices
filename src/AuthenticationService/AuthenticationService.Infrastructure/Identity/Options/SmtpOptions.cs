using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Identity.Options
{
    public class SmtpOptions
    {

        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;



        public string FromAddress { get; set; } = string.Empty; 

        public string DisplayName { get; set; } = "Fitness App";
        public bool EnableSsl { get; set; } = true;
    }
}
