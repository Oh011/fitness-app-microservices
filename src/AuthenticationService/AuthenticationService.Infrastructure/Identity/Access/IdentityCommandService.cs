using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Common.Errors;
using AuthenticationService.Infrastructure.Identity.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace AuthenticationService.Infrastructure.Identity.Access
{
    internal class IdentityCommandService(IUnitOfWork unitOfWork)
    {


       public async Task<Result> CreateUserAsync(ApplicationUser user)
        {

            var repository=unitOfWork.GetRepository<ApplicationUser>();

            try
            {
                await repository.AddAsync(user);

                await unitOfWork.Commit();


            }

            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlEx)
                {
                    if (sqlEx.Number == 2601 || sqlEx.Number == 2627)
                    {



                        return Result.Conflict(AuthErrorMessages.USER_ALREADY_EXISTS);
                    }
                }

                throw; // unknown DB error
            }


            return Result.Success("User created successfully.");


        }

    }
}
