
using MediatR;
using Shared.Results;

namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public record ConfirmEmailCommand:IRequest<Result>
    {

      
        public string Token { get; init; }  
    }
}
