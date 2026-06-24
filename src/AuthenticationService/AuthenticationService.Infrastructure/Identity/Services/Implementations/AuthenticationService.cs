
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Abstractions.Security.Services;
using AuthenticationService.Application.Common.Errors;
using AuthenticationService.Application.Features.Authentication.Dtos.Internal;
using AuthenticationService.Infrastructure.Identity.Access;
using AuthenticationService.Infrastructure.Identity.Models;
using AuthenticationService.Infrastructure.Identity.Security;
using Shared.Results;

namespace AuthenticationService.Infrastructure.Identity.Services.Implementations
{
    internal class AuthenticationService( UserQueryService queryService, IUnitOfWork unitOfWork, IdentityCommandService commandService, Hasher passwordHasher, CredentialValidator validator) : IAuthenticationService
    {

        private const int MaxFailedAccessAttempts = 5; // could be configurable


        private bool CheckPassword(ApplicationUser user, string password)
        {

            return passwordHasher.VerifyWithSalt(password, user.PasswordHash, user.PasswordSalt);
        }



        private bool IsLockedOut(ApplicationUser user)
        {
            if (user.LockoutEnd.HasValue && user.IsLocked==true && user.LockoutEnd <= DateTime.UtcNow)
            {
                
                ResetAccessFailedCount(user); // unlock the user    
                return false;
            }

            if (!user.IsLocked) return false;

            return true;
        }

        private void AccessFailed(ApplicationUser user)
        {
            user.RetryCount++;
            if (user.RetryCount >= MaxFailedAccessAttempts)
            {
                user.IsLocked = true;
                user.LockoutEnd = DateTime.UtcNow.AddMinutes(2);
            }
        }

        private void ResetAccessFailedCount(ApplicationUser user)
        {
            user.RetryCount = 0;
            user.IsLocked = false;
            user.LockoutEnd = null;
        }







        public async Task<Result<UserDto>> LogInAsync(string Email, string Password, string DeviceId)
        {

            var user = await queryService.GetByEmailAsync(Email);
          

            if (user == null) return Result<UserDto>.Unauthorized(AuthErrorMessages.INVALID_CREDENTIALS);



            if (!user.IsEmailConfirmed)
                return Result<UserDto>.Forbidden(AuthErrorMessages.EMAIL_NOT_CONFIRMED);


            if ( IsLockedOut(user))
            {
                return Result<UserDto>.Forbidden(AuthErrorMessages.ACCOUNT_LOCKED);
            }




            if (!CheckPassword(user, Password))
            {
                AccessFailed(user);
                await unitOfWork.Commit(); // persist failed attempt

                if (IsLockedOut(user))
                {
                    await unitOfWork.Commit(); // persist lockout reset if it just expired
                    return Result<UserDto>.Forbidden(AuthErrorMessages.ACCOUNT_LOCKED);
                }

                return Result<UserDto>.Unauthorized(AuthErrorMessages.INVALID_CREDENTIALS);
            }



            ResetAccessFailedCount(user);      // mutate user
            await unitOfWork.Commit();         // save user changes inside transaction

          

            return Result<UserDto>.Success(new UserDto
            {

                Email = Email,
                Id = user.Id,
                UserName = user.UserName
            });




        }




        



        public async Task<Result<long>> RegisterAsync(string Email, string Password, string UserName, string role = "User")
        {


            var validationResult = validator.ValidateCredentials(Email, Password);   


            if(validationResult.IsSuccess== false && validationResult.ValidationErrors!=null)
            {
                return Result<long>.Validation(validationResult.ValidationErrors, "User registration failed due to validation errors.");
            }



            var userExists = await queryService.EmailExistsAsync(Email);

            if (userExists)
            {
                return Result<long>.Conflict(AuthErrorMessages.USER_ALREADY_EXISTS);
            }


            var hash = passwordHasher.HashWithSalt(Password);

            var user = new ApplicationUser
            {
                UserName= UserName, 
                Email = Email,
                PasswordHash = hash.Hash,
                PasswordSalt = hash.Salt,

            };


            var result = await commandService.CreateUserAsync(user);


            if (!result.IsSuccess)
            {
                return Result<long>.FromResult(result);
            }

      

            return Result<long>.Success(user.Id);
        }

    
       

      


    }
}
