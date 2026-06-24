using AuthenticationService.Application.Abstractions.Bcakground;
using AuthenticationService.Application.Abstractions.Messaging.Email;
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Abstractions.Security.Services;
using AuthenticationService.Application.Abstractions.Services;
using AuthenticationService.Application.Common.Errors;
using AuthenticationService.Application.Common.Messages.Success;
using AuthenticationService.Application.Features.Authentication.Dtos.Internal;
using AuthenticationService.Infrastructure.EmailTemplates.Builder;
using AuthenticationService.Infrastructure.Identity.Access;
using AuthenticationService.Infrastructure.Identity.Models;
using AuthenticationService.Infrastructure.Identity.Security;
using AuthenticationService.Infrastructure.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared;
using Shared.Results;
using System.Security.Cryptography;

namespace AuthenticationService.Infrastructure.Identity.Services.Implementations
{
    internal class PasswordService(IUrlBuilder urlBuilder, IEmailTemplateBuilder templateBuilder, IBackgroundJobQueue backGroundJobQueue, IUnitOfWork unitOfWork , UserQueryService userQueryService, CredentialValidator validator,   Hasher hasher) : IPasswordService
    {


        private bool CheckPassword(ApplicationUser user, string password)
        {

            return hasher.VerifyWithSalt(password, user.PasswordHash, user.PasswordSalt);
        }

        public async Task<Result<long>> ChangePasswordAsync(long UserId, string OldPassword, string newPassword)
        {

            var user = await userQueryService.GetByIdAsync(UserId);


            if (user == null) return Result<long>.Validation(AuthErrorMessages.INVALID_CREDENTIALS);




            if (!CheckPassword(user, OldPassword))
            {

                return Result<long>.Validation(AuthErrorMessages.INVALID_CREDENTIALS);
            }

            var passwordValidationResult = validator.ValidatePassword(newPassword);
            if (passwordValidationResult.IsSuccess == false)
            {
                return Result<long>.FromResult( passwordValidationResult);
            }


            var HashResult = hasher.HashWithSalt(newPassword);

            user.PasswordHash = HashResult.Hash;
            user.PasswordSalt = HashResult.Salt;

            user.PasswordHash = HashResult.Hash;
            user.PasswordSalt = HashResult.Salt;

            await unitOfWork.Commit();


            return Result<long>.Success(user.Id);


        }


        private async Task<Result<string>> GeneratePasswordResetTokenAsync(long userId,string email)
        {
            var stateRepo = unitOfWork.GetRepository<PasswordResetState>();
            var state = await stateRepo.FirstOrDefaultAsync(x => x.UserId == userId);

            if (state == null)
            {
                state = new PasswordResetState {Email=email, UserId = userId, ResendCount = 0, LastSentAt = null };
                await stateRepo.AddAsync(state);
            }

            if (state.LastSentAt.HasValue && state.LastSentAt.Value <= DateTime.UtcNow.AddHours(-1))
                state.ResendCount = 0;

            if (state.ResendCount >= 5)
                return Result<string>.Conflict("Too many requests. Try again later.");

            var repo = unitOfWork.GetRepository<PasswordResetTokens>();

            // invalidate any existing unused tokens for this user
            var oldTokens =  await repo.GetAllAsIQueryableAsync().Where(t => t.UserId == userId && !t.IsUsed).ToListAsync();
            foreach (var old in oldTokens)
                old.IsUsed = true;

            var rawBytes = RandomNumberGenerator.GetBytes(64);
            var rawToken = Base64UrlEncoder.Encode(rawBytes);

            await repo.AddAsync(new PasswordResetTokens
            {
                UserId = userId,
                TokenHash = hasher.HashWithoutSalt(rawToken),
                ExpiresAt = DateTime.UtcNow.AddMinutes(15),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            });

            state.ResendCount++;
            state.LastSentAt = DateTime.UtcNow;

            await unitOfWork.Commit();
            return Result<string>.Success(rawToken);
        }



        public async Task<Result<UserDto>> ResetPasswordAsync(string token, string newPassword)
        {
            // 1. validate new password strength (reuse CredentialValidator)
            var validationResult = validator.ValidatePassword(newPassword);
            if (!validationResult.IsSuccess)
                return Result<UserDto>.Validation(validationResult.ValidationErrors, "Password reset failed due to validation errors.");

            // 2. find & validate token
            var hashedInput = hasher.HashWithoutSalt(token);
            var tokenRecord = await unitOfWork.GetRepository<PasswordResetTokens>()
                .FirstOrDefaultAsync(t => t.TokenHash == hashedInput && !t.IsUsed && t.ExpiresAt > DateTime.UtcNow);

            if (tokenRecord == null)
                return Result<UserDto>.Validation(AuthErrorMessages.INVALID_OR_EXPIRED_RESET_CODE);

            var user = await userQueryService.GetByIdAsync(tokenRecord.UserId);
            if (user == null)
                return Result<UserDto>.Validation(AuthErrorMessages.INVALID_OR_EXPIRED_RESET_CODE);

            // 3. update password
            var newHash = hasher.HashWithSalt(newPassword);
            user.PasswordHash = newHash.Hash;
            user.PasswordSalt = newHash.Salt;

            // 4. consume token
            tokenRecord.IsUsed = true;

            // 5. unlock account if it was locked (user proved identity via email)
            //ResetAccessFailedCount(user);

            await unitOfWork.Commit();



            return Result<UserDto>.Success(new UserDto
            {
                Id=user.Id,
                UserName= user.UserName,    
                Email=user.Email,   
            });
        }


        public async Task<Result> ForgotPasswordAsync(string email)
        {
            var user = await userQueryService.GetByEmailAsync(email);

            // Always return the same success message — don't leak whether the email exists
            if (user == null || !user.IsEmailConfirmed)
                return Result.Success(AuthSuccessMessages.PASSWORD_RESET_EMAIL_SENT_IF_EXISTS);

            var tokenResult = await GeneratePasswordResetTokenAsync(user.Id,user.Email);
            if (!tokenResult.IsSuccess)
            {
                // rate-limited — still return generic success to avoid leaking info,
                // but don't actually send another email
                return Result.Success(AuthSuccessMessages.PASSWORD_RESET_EMAIL_SENT_IF_EXISTS);
            }

            backGroundJobQueue.Enqueue<IEmailService>(e => e.SendEmailAsync(new EmailMessage
            {
                To = user.Email,
                Subject = "Reset Your Password",
                Body = templateBuilder.BuildPasswordResetTemplate(
                    urlBuilder.BuildPasswordResetUrl(tokenResult.Value), user.UserName),
                IsHtml = true
            }));

            return Result.Success(AuthSuccessMessages.PASSWORD_RESET_EMAIL_SENT_IF_EXISTS);
        }






    }
}
