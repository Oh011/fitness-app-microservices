
using AuthenticationService.Application.Features.Authentication.Commands;
using FluentValidation;


namespace AuthenticationService.Application.Features.Authentication.Validators
{

    public class RegisterCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterCommandValidator()
        {


            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email cannot be empty.")
                .EmailAddress().WithMessage("Email format is invalid.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username cannot be empty.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters.");

         
        }
    }
}
