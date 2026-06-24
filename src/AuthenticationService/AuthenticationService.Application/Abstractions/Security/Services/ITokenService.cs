using AuthenticationService.Application.Features.Authentication.Dtos.Internal;
using AuthenticationService.Application.Features.Authentication.Dtos.Results;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Abstractions.Security.Services
{
    public interface ITokenService
    {

        public Task<Result> RevokeAllUserSessionsAsync(long userId);

       
        public Task<RefreshTokenDto> IssueRefreshTokenForDeviceAsync(long userId, string DeviceId);


        public Task<Result> RevokeDeviceSessionAsync(string ? refershtoken, string deviceId);

        public Task<Result<LogInUserResponse>> RotateAccessTokenAsync(string refreshToken, string deviceId);
        public Task<string> IssueAccessTokenAsync(long userId, string Email, string UserName);
    }
}

