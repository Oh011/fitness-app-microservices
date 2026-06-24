using AuthenticationService.Application.Features.Authentication.Commands;
using Shared.Results;
using MediatR;
using AuthenticationService.Application.Abstractions.Security.Services;

namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class ResendConfirmationEmailCommandHandler : IRequestHandler<ResendConfirmationEmailCommand, Result>
    {

        private readonly IEmailVerificationService _emailVerificationService;


        public ResendConfirmationEmailCommandHandler(IEmailVerificationService emailVerificationService)
        {

            this._emailVerificationService = emailVerificationService;
        }
        public async Task<Result> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
        {


            return await _emailVerificationService.SendConfirmationEmail(request.Email);
        }
    }
}
