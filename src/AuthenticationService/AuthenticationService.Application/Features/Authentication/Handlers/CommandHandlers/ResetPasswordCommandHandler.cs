using AuthenticationService.Application.Abstractions.Bcakground;
using AuthenticationService.Application.Abstractions.Messaging.Email;
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Abstractions.Services;
using AuthenticationService.Application.Common.Messages.Success;
using AuthenticationService.Application.Features.Authentication.Commands;
using Shared.Results;
using MediatR;
using Shared;
using AuthenticationService.Application.Abstractions.Security.Services;


namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordService passwordService;
        private readonly ITokenService tokenService;
        private readonly IBackgroundJobQueue backgroundJobQueue;
        private readonly IEmailTemplateBuilder emailTemplateBuilder;



        public ResetPasswordCommandHandler(IEmailTemplateBuilder emailTemplateBuilder, IUnitOfWork unitOfWork, IPasswordService passwordService, ITokenService tokenService, IBackgroundJobQueue backgroundJobQueue)
        {
            this.unitOfWork = unitOfWork;
            this.passwordService = passwordService;
            this.tokenService = tokenService;
            this.backgroundJobQueue = backgroundJobQueue;
            this.emailTemplateBuilder = emailTemplateBuilder;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {



            await unitOfWork.BeginTransactionAsync();


            try
            {


                var passwordRestResult = await passwordService.ResetPasswordAsync(request.Token, request.NewPassword);



                if (passwordRestResult.IsSuccess == false)
                {

                    await unitOfWork.RollbackTransactionAsync();
                    return Result.FromResult(passwordRestResult);

                }

                var user = passwordRestResult.Value;

                var tokenRevoactionResult = await tokenService.RevokeAllUserSessionsAsync(passwordRestResult.Value.Id);


                await unitOfWork.CommitTransactionAsync();
                // 7. optionally: notify user via email that their password was changed
                backgroundJobQueue.Enqueue<IEmailService>(e => e.SendEmailAsync(new EmailMessage
                {
                    To = user.Email,
                    Subject = "Your password was changed",
                    Body = emailTemplateBuilder.BuildPasswordChangedTemplate(user.UserName),
                    IsHtml = true
                }));

            }


            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result.Failure("An error occurred while resting Password ");
            }


            return Result.Success(AuthSuccessMessages.PASSWORD_RESET_SUCCESS);
        }
    }
}
