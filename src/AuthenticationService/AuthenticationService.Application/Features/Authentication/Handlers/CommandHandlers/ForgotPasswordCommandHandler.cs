using AuthenticationService.Application.Features.Authentication.Commands;
using   Shared.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using AuthenticationService.Application.Abstractions.Security.Services;

namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {

        private readonly IPasswordService passwordService;


        public ForgotPasswordCommandHandler(IPasswordService passwordService)
        {
                this.passwordService=passwordService;
        }
        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {


            return await passwordService.ForgotPasswordAsync(request.email);

            
        }
    }
}
