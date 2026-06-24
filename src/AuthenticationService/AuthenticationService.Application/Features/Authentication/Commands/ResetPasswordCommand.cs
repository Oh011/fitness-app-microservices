
using MediatR;
using Shared.Results;

namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public record ResetPasswordCommand(
     string Token,
     string NewPassword
 ):IRequest<Result>;

}
