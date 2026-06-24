using Shared.Results;
using MediatR;


namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public record ChangePasswordCommand:IRequest<Result>
    {


        public string Email { get; init; }   


        public string CurrentPassword { get; init; } 


        public string NewPassword { get; init; }
    }
}
