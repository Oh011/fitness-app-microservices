using AuthenticationService.Application.Abstractions.Identity;
using AuthenticationService.Application.Common.Messages.Success;
using AuthenticationService.Application.Features.Authentication.Commands;
using Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using AuthenticationService.Application.Abstractions.Security.Services;

namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class LogOutCommandHandler : IRequestHandler<LogOutCommanad, Result>
    {


        private readonly ICurrentUserService currentUserService;
        private readonly ITokenService tokenService;


        public LogOutCommandHandler(ICurrentUserService currentUserService, ITokenService tokenService)
        {
            this.currentUserService = currentUserService;
            this.tokenService = tokenService;
        }

        public async Task<Result> Handle(LogOutCommanad request, CancellationToken cancellationToken)
        {

            var userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated.");


            await tokenService.RevokeDeviceSessionAsync(request.RefreshToken , request.DeviceId);


            return Result.Success(AuthSuccessMessages.LOGOUT_SUCCESS);
        }
    }
}
