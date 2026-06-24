using AuthenticationService.Application.Abstractions.Bcakground;
using AuthenticationService.Application.Abstractions.Messaging.Email;
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Abstractions.Services;
using AuthenticationService.Application.Common.Errors;
using AuthenticationService.Application.Common.Messages.Success;
using AuthenticationService.Infrastructure.EmailTemplates.Builder;
using AuthenticationService.Infrastructure.Identity.Access;
using AuthenticationService.Infrastructure.Identity.Models;
using AuthenticationService.Infrastructure.Identity.Security;
using AuthenticationService.Infrastructure.Services;
using AuthenticationService.Application.Abstractions.Security.Services;
using Microsoft.IdentityModel.Tokens;
using Shared.Results;
using System.Security.Cryptography;
using System.Text;
using Shared;

namespace AuthenticationService.Infrastructure.Identity.Services.Implementations
{
    internal class EmailVerificationService(IEmailTemplateBuilder templateBuilder, IUrlBuilder urlBuilder, IBackgroundJobQueue backGroundJobQueue, UserQueryService QueryService, Hasher hasher, IUnitOfWork unitOfWork) : IEmailVerificationService
    {



        public async Task<Result> ConfirmEmailAsync(string token)
        {

            var hashedInput = hasher.HashWithoutSalt(token);

            var tokenRecord = await unitOfWork.GetRepository<EmailConfirmationTokens>()
                .FirstOrDefaultAsync(t =>
                    t.TokenHash==hashedInput &&
                    !t.IsUsed &&
                    t.ExpiresAt > DateTime.UtcNow);



            if (tokenRecord == null)
                return Result.Validation(
                    AuthErrorMessages.INVALID_OR_EXPIRED_CONFIRMATION_CODE);


            var user = await QueryService.GetByIdAsync(tokenRecord.UserId);



            if (user.IsEmailConfirmed)
                return Result.Success(AuthSuccessMessages.EMAIL_ALREADY_CONFIRMED);



            if (tokenRecord.TokenHash != hashedInput)
                return Result.Validation(
                        AuthErrorMessages.INVALID_OR_EXPIRED_CONFIRMATION_CODE);




            tokenRecord.IsUsed = true;
            user.IsEmailConfirmed = true;

            await unitOfWork.Commit();

           

            return Result.Success(
                AuthSuccessMessages.EMAIL_CONFIRMED
        );
        }



        private async Task<Result<string>> GenerateEmailConfirmationTokenAsync(long UserId,string email)
        {


            var stateRepo = unitOfWork.GetRepository<EmailVerificationState>();

            var state = await stateRepo.FirstOrDefaultAsync(x => x.UserId == UserId);

            if (state == null)
            {
                state = new EmailVerificationState
                {
                    UserId =UserId,
                    Email = email,
                    ResendCount = 0,
                    LastSentAt = null
                };

                await stateRepo.AddAsync(state);
            }


            if (state.LastSentAt.HasValue && state.LastSentAt.Value <= DateTime.UtcNow.AddHours(-1))
            {
                state.ResendCount = 0; // reset window
            }


            if (state.ResendCount >= 5)
            {
                return Result<string>.Conflict(
                    "Too many requests. Try again later.");
            }



            var repo = unitOfWork.GetRepository<EmailConfirmationTokens>();



            var rawBytes = RandomNumberGenerator.GetBytes(64);
            var urlSafeToken = Base64UrlEncoder.Encode(rawBytes);  // this is your token string

            var oldToken = await repo.FirstOrDefaultAsync(t => t.UserId == UserId && !t.IsUsed);
            if (oldToken != null)
                oldToken.IsUsed = true;

            var newToken = new EmailConfirmationTokens
            {
                UserId = UserId,
                TokenHash = hasher.HashWithoutSalt(urlSafeToken), 
                ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false,
                CreatedAt = DateTime.UtcNow
            };

            await repo.AddAsync(newToken);



            state.ResendCount++;
            state.LastSentAt = DateTime.UtcNow;



            await unitOfWork.Commit();

            return Result<string>.Success(urlSafeToken);



        }
    
        public async Task<Result> SendConfirmationEmail(string email)
        {
            

            var user=await QueryService.GetByEmailAsync(email);


            if (user == null)
                return Result.NotFound(AuthErrorMessages.USER_NOT_FOUND);


            if (user.IsEmailConfirmed)
                return Result.Success(AuthSuccessMessages.EMAIL_ALREADY_CONFIRMED);


            var emailOtpResult = await GenerateEmailConfirmationTokenAsync(user.Id,user.Email);



            if (emailOtpResult.IsSuccess == false)
                return Result.Failure(emailOtpResult.Message);

            var body = templateBuilder.BuildEmailConfirmationTemplate(
                    urlBuilder.BuildEmailConfirmationUrl(emailOtpResult.Value), user.UserName);

            backGroundJobQueue.Enqueue<IEmailService>(e => e.SendEmailAsync(new EmailMessage
            {
                To = user.Email,
                Subject = "Confirm Your Email — Digital Wallet",
                Body = body,
                IsHtml = true
            }));


            return Result.Success("Confirmation email sent successfully."); 

        }


    }
}
