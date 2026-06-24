using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Dtos.Results
{

    public record AuthenticationResponse(
        string Email,
        long UserId,
        string UserName,
        string AccessToken
    );



}
