using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Abstractions.Security.Services;
using AuthenticationService.Application.Common.Errors;
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Results;


namespace AuthenticationService.Infrastructure.Identity.Services.Implementations
{
    internal class RoleService(IUnitOfWork unitOfWork) : IRoleService
    {


        public async Task<Result> AssignDefaultRoleAsync(long userId)
       => await AssignRoleAsync(userId, "User"); // default role name

        public async Task<Result> AssignRoleAsync(long userId, string roleName)
        {
            var user = await unitOfWork.GetRepository<ApplicationUser>().GetById(userId);
            if (user is null)
                return Result.NotFound(AuthErrorMessages.USER_NOT_FOUND);

            var role = await unitOfWork.GetRepository<Role>()
                .FirstOrDefaultAsync(r => r.Name == roleName);

        
                if (role is null)
                    throw new InvalidOperationException("Default role 'User' not found in database. Ensure seeding is executed.");

            user.RoleId = role.Id;
            await unitOfWork.Commit();
            return Result.Success("Role assigned successfully.");
        }

        public async Task<IEnumerable<string>> GetAllRolesAsync()
        {
            var repository= unitOfWork.GetRepository<Role>();

            return await repository.GetAllAsIQueryableAsync().Select(r => r.Name).ToListAsync();
        }

        public Task<IEnumerable<string>> GetRolesByUserIdAsync(long userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> RevokeRoleAsync(long userId, string roleName)
        {
            throw new NotImplementedException();
        }


       


    }
}
