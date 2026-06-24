using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Models
{
    public class EmailVerificationState
    {


        public int Id { get; set; }


        public string Email { get; set; }

        public ApplicationUser user { get; set; }


        public long UserId { get; set; }    


        public int ResendCount { get; set; } = 0;   

        public DateTime   ? LastSentAt { get; set; } 
    }
}
