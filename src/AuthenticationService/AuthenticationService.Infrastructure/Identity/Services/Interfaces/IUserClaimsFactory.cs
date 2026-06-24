
using AuthenticationService.Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Infrastructure.Identity.Services.Interfaces
{
    internal interface IUserClaimsFactory
    {
        Task<List<Claim>> CreateClaimsAsync(ApplicationUser user);
        
    }
}
