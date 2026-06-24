
using AuthenticationService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Models
{
    public class ApplicationUser:BaseEntity
    {


        public long Id { get; set; }    


        public Role ? Role { get; set; }

        public int ? RoleId { get; set; } 
        public string UserName { get; set; }



        public bool IsLocked { get; set; } = false;


        public DateTime ? LockoutEnd { get; set; }    


        public ICollection<RefreshToken> refreshTokens { get; set; }=new HashSet<RefreshToken>();
        public ICollection<EmailConfirmationTokens> EmailconfirmationTokens { get; set; }=new HashSet<EmailConfirmationTokens>();


        public ICollection<PasswordResetTokens> PasswordResetTokens { get; set; }=new HashSet<PasswordResetTokens>();


        public bool IsEmailConfirmed { get; set; } = false;


        public bool TwoFactorEnabled { get; set; } = false;
        public string? TwoFactorSecret { get; set; }


        public int RetryCount { get; set; } = 0;

        public string PasswordSalt { get; set; }

        public string PasswordHash { get; set; }


        public string Email { get; set; }
    }
}
