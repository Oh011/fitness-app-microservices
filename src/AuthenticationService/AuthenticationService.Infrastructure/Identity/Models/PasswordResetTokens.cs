using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Models
{
    public class PasswordResetTokens
    {

        public Guid Id { get; set; }


        public string TokenHash { get; set; }



        public DateTime ExpiresAt { get; set; }


        public bool IsUsed { get; set; } = false;


        public ApplicationUser user { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public long UserId { get; set; }
    }
}
