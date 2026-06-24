
using MediatR;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Features.Authentication.Commands
{
    public class ResendConfirmationEmailCommand:IRequest<Result>
    {

        public string Email { get; init; }
    }
}
