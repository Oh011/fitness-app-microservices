using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Dtos.Requests
{
    public record LoginUserRequest(string Email, string Password, string DeviceId);
}
