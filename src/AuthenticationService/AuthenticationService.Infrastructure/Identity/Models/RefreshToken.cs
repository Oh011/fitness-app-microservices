using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Models
{
    
    public class RefreshToken
    {

        public int Id { get; set; }

        public string TokenHash { get; set; } = null!;

        public string DeviceId { get; set; }

        public long UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public bool IsRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;



        public void Revoke()
        {

            if (IsRevoked) { return; }

            this.IsRevoked = true;
            this.RevokedAt = DateTime.UtcNow;
        }


    }
}
