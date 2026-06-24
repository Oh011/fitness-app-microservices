using AuthenticationService.Application.Features.Authentication.Dtos.Results;
using MediatR;
using Shared.Results;

namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public class RefreshAccessTokenCommand:IRequest<Result<LogInUserResponse>>
    {



        public string ? RefreshToken { get;set ; }


        public string DeviceId { get; set; }

    }
}
