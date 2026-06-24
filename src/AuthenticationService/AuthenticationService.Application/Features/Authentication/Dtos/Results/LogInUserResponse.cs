using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Dtos.Results
{

    public record LogInUserResponse(
       string AccessToken,
    string RefreshToken,
    DateTime RefreshTokenExpiration,
    string Email,
    long UserId,
    string UserName
    );
}
