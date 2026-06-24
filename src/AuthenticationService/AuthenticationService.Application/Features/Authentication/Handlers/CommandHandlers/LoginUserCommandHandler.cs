using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Features.Authentication.Commands;
using AuthenticationService.Application.Features.Authentication.Dtos.Results;
using Shared.Results;
using MediatR;
using AuthenticationService.Application.Abstractions.Security.Services;

namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LogInUserResponse>>
    {




        private readonly ITokenService tokenService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUnitOfWork unitOfWork;



        public LoginUserCommandHandler( ITokenService tokenService,IAuthenticationService authenticationService,IUnitOfWork unitOfWork)
        {

            this.tokenService = tokenService;
            this._authenticationService = authenticationService;
            this.unitOfWork = unitOfWork;


        }
      
        public async Task<Result<LogInUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {



            await unitOfWork.BeginTransactionAsync();


            try
            {

                var result = await _authenticationService.LogInAsync(request.Email, request.Password, request.DeviceId);
                if (result.IsSuccess == false)
                {
                    await unitOfWork.RollbackTransactionAsync();
                    return Result<LogInUserResponse>.FromResult(result);
                }



                var refreshToken = await tokenService
                .IssueRefreshTokenForDeviceAsync(result.Value.Id, request.DeviceId); // saves token changes inside transaction

                // Build response after transaction completes
                var accessToken = await tokenService.IssueAccessTokenAsync(result.Value.Id, request.Email, result.Value.UserName);

                var response = new LogInUserResponse(
                    accessToken, refreshToken.Token,
                    refreshToken.Expiration,
                    request.Email, result.Value.Id, result.Value.UserName
                );
                await unitOfWork.CommitTransactionAsync();

                return Result<LogInUserResponse>.Success(response);




            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result<LogInUserResponse>.Failure($"An error occurred while processing the login request");


            }

            //return await sessionOrchestrator.LoginAsync(request);

        }
    }
}
