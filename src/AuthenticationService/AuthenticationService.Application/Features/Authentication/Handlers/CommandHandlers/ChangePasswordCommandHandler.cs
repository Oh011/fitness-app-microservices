using AuthenticationService.Application.Abstractions.Identity;
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Common.Messages.Success;
using AuthenticationService.Application.Features.Authentication.Commands;
using Shared.Results;
using MediatR;
using AuthenticationService.Application.Abstractions.Security.Services;

namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {


        private readonly IUnitOfWork unitOfWork;
        private readonly IPasswordService passwordService;
        private readonly ICurrentUserService currentUserService;
        private readonly ITokenService tokenService;



     

        public ChangePasswordCommandHandler( IUnitOfWork unitOfWork, IPasswordService passwordService, ICurrentUserService currentUserService, ITokenService tokenService)
        {

            this.unitOfWork = unitOfWork;
            this.passwordService = passwordService;
            this.currentUserService = currentUserService;
            this.tokenService = tokenService;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {


            var userId = currentUserService.UserId
         ?? throw new UnauthorizedAccessException("User is not authenticated.");


            await unitOfWork.BeginTransactionAsync();


            try
            {

                var PasswordCahngeResult = await passwordService.
                      ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);



                if (PasswordCahngeResult.IsSuccess == false)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return Result.FromResult(PasswordCahngeResult);
                }


                var tokenRevokationResult = await tokenService.RevokeAllUserSessionsAsync(PasswordCahngeResult.Value);


                if (tokenRevokationResult.IsSuccess == false)
                {

                    await unitOfWork.RollbackTransactionAsync();
                    return tokenRevokationResult;
                }



                await unitOfWork.CommitTransactionAsync();



            }

            catch (Exception ex)
            {

                await unitOfWork.RollbackTransactionAsync();
                return Result.Failure("An error occurred while Cahnging Password");
            }




            return Result.Success(AuthSuccessMessages.PASSWORD_CHANGED);

        }
    }
}
