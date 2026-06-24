using AuthenticationService.Application.Features.Authentication.Dtos.Results;

using MediatR;
using Shared.Results;


namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public record LoginUserCommand:IRequest<Result<LogInUserResponse>>
    {

        public string Email { get; init; }

        public string Password { get; init; }


        public string DeviceId { get; init; }   
    }
}
