using AuthenticationService.Application.Features.Authentication.Dtos.Internal;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService .Application.Abstractions.Security.Services
{
    public interface IPasswordService
    {



        public Task<Result> ForgotPasswordAsync(string email);
        public Task<Result<UserDto>> ResetPasswordAsync(string token, string newPassword);
        public Task<Result<long>> ChangePasswordAsync(long UserId, string currentPassword, string newPassword);
    }
}
