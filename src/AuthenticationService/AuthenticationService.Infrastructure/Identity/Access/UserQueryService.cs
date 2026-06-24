using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Access
{
    internal class UserQueryService(IUnitOfWork unitOfWork)
    {



        public async Task<ApplicationUser?> GetByIdAsync(long id)
        {

           return await unitOfWork.GetRepository<ApplicationUser>().GetById(id);

        }


        public async Task<string> GetRoleNameByUserIdAsync(long userId)
        {

            var user = await unitOfWork.GetRepository<ApplicationUser>().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return string.Empty;
            var role = await unitOfWork.GetRepository<Role>().GetById(user.RoleId);
            return role?.Name ?? string.Empty;
        }


        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {

            return await unitOfWork.GetRepository<ApplicationUser>().FirstOrDefaultAsync
                (u => u.Email == email);

        }
       public async Task<bool> EmailExistsAsync(string email)
        {

            return await unitOfWork.GetRepository<ApplicationUser>().
               ExistsAsync(u => u.Email == email);

        }
    }
}
