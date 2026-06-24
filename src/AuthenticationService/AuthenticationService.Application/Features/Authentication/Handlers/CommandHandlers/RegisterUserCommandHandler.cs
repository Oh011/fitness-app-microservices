using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Abstractions.Security.Services;
using AuthenticationService.Application.Common.Messages.Success;
using AuthenticationService.Application.Features.Authentication.Commands;
using Shared.Results;

using MediatR;

namespace AuthenticationService.Application.Features.Authentication.Handlers.CommandHandlers
{
    internal class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailVerificationService _emailVerificationService;
        private readonly IRoleService roleService;



        public RegisterUserCommandHandler( IUnitOfWork unitOfWork, IAuthenticationService authenticationService, IEmailVerificationService emailVerificationService, IRoleService roleService)
        {

            this.unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _emailVerificationService = emailVerificationService;
            this.roleService = roleService;
        }

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {



            await unitOfWork.BeginTransactionAsync();



            try
            {

                var authResult = await _authenticationService.RegisterAsync(request.Email, request.Password, request.UserName);


                if (!authResult.IsSuccess)
                {


                    await unitOfWork.RollbackTransactionAsync();
                    return authResult;

                }




                var roleResult = await roleService.AssignDefaultRoleAsync(authResult.Value);



                if (!roleResult.IsSuccess)
                {

                    await unitOfWork.RollbackTransactionAsync();
                    return roleResult;
                }





                //Doamin User Creation and other related operations can be done here if needed



                var emailVerficationResult = await _emailVerificationService.SendConfirmationEmail(request.Email);


                if (!emailVerficationResult.IsSuccess)
                    return emailVerficationResult;

                await unitOfWork.CommitTransactionAsync();



                return Result.Created(AuthSuccessMessages.USER_REGISTERED);

            }


            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                return Result.Failure("An error occurred while registering the user: ");


            }


            //return await registrationOrchestrator.RegisterAsync(request);

        }

    }
}
