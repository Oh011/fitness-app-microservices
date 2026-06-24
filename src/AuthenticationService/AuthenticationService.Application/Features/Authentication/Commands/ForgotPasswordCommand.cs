
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public record ForgotPasswordCommand(string email):IRequest<Result>;
   
}
