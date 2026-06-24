
using AuthenticationService.Application.Features.Authentication.Commands;
using Shared.Results;
using MediatR;
using AuthenticationService.Application.Abstractions.Security.Services;


namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
    {



        private readonly IEmailVerificationService _emailVerificationService;



        public ConfirmEmailCommandHandler(IEmailVerificationService emailVerificationService)
        {

            this._emailVerificationService = emailVerificationService;
        }
        public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {

         

            return await  _emailVerificationService.ConfirmEmailAsync(request.Token);


        }
    }
}
