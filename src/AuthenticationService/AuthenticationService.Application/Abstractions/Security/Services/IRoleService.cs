using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Abstractions.Security.Services
{


    // Application layer
    public interface IRoleService
    {
        // called during registration — assigns the default "User" role
        Task<Result> AssignDefaultRoleAsync(long userId);

        // called by admin — assign any named role
        Task<Result> AssignRoleAsync(long userId, string roleName);

        // called by admin — revoke a role
        Task<Result> RevokeRoleAsync(long userId, string roleName);

        // called by IUserClaimsProvider — to put roles in JWT claims
        Task<IEnumerable<string>> GetRolesByUserIdAsync(long userId);

        // called by admin — list all available roles in the system
        Task<IEnumerable<string>> GetAllRolesAsync();
    }
}
