
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public record LogOutCommanad:IRequest<Result>
    {



   
        public string ? RefreshToken { get; set; }  
        public string DeviceId { get; init; }
        
    }
}
