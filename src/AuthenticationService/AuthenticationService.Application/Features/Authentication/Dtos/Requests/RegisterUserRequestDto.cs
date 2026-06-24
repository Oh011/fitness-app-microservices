using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Dtos.Requests
{
    public record RegisterUserRequestDto(string Email, string Password, string UserName);
}
