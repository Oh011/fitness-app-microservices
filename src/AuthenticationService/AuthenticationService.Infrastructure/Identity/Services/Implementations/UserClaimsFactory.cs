
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Infrastructure.Identity.Access;
using AuthenticationService.Infrastructure.Identity.Models;
using AuthenticationService.Infrastructure.Identity.Services.Interfaces;
using System.Security.Claims;


namespace AuthenticationService.Infrastructure.Identity.Services.Implementations
{
    internal class UserClaimsFactory : IUserClaimsFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserQueryService userQueryService;
        


        public UserClaimsFactory(
            IUnitOfWork unitOfWork,UserQueryService userQueryService
            )
        {
            _unitOfWork = unitOfWork;
            this.userQueryService = userQueryService;

        }   
        public async Task<List<Claim>> CreateClaimsAsync(ApplicationUser user)
        {
            //var employeeRepository =
            //    _unitOfWork.GetRepository<WalletUser>();



            //var employeeId = await employeeRepository
            //    .FirstOrDefaultAsync(
            //        e => e.AuthUserId == user.Id,
            //        e => (int?)e.Id);

            //if (employeeId is null)
            //    throw new UnauthorizedAccessException(
            //        "Employee profile not found.");

            var roles = await userQueryService.GetRoleNameByUserIdAsync(user.Id);

            return new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new Claim(
                ClaimTypes.Email,
                user.Email!),

            new Claim(
                ClaimTypes.Role,
                roles),

           
        };
        }



    }


    public static class CustomClaimTypes
    {
        public const string WalletUserId = "WalletUserId";
    }
}
