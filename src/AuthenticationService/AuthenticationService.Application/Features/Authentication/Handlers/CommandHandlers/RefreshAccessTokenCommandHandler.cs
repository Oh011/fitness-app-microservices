using AuthenticationService.Application.Features.Authentication.Commands;
using AuthenticationService.Application.Features.Authentication.Dtos.Results;
using Shared.Results;
using MediatR;
using AuthenticationService.Application.Abstractions.Security.Services;


namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class RefreshAccessTokenCommandHandler : IRequestHandler<RefreshAccessTokenCommand, Result<LogInUserResponse>>
    {


        private readonly ITokenService tokenService;


        public RefreshAccessTokenCommandHandler(ITokenService tokenService)
        {
            this.tokenService = tokenService;   
        }

        public async Task<Result<LogInUserResponse>> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {


            return await tokenService.RotateAccessTokenAsync(request.RefreshToken, request.DeviceId);


        }
    }
}
