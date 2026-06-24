
using AuthenticationService.Infrastructure.Identity.Options;
using Microsoft.Extensions.Options;
using Shared.Errors;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AuthenticationService.Infrastructure.Identity.Security
{

    internal class CredentialValidator
    {


        private readonly PasswordOptions _options;

        public CredentialValidator(IOptions<PasswordOptions> options)
        {
            _options = options.Value;
        }

        public Result ValidatePassword(string? password)
        {

            var validationErrors = new Dictionary<string, List<ValidationErrorDetail>>();
            validationErrors["Password"] =new List<ValidationErrorDetail>();
            

            if (string.IsNullOrWhiteSpace(password))
            {
                validationErrors["Password"].Add(new ValidationErrorDetail("Password cannot be empty."));
                return Result.Validation(validationErrors);
            }

            if (password.Length < _options.RequiredLength)
                validationErrors["Password"].Add(new ValidationErrorDetail($"Password must be at least {_options.RequiredLength} characters long.")     );

            if (_options.RequireLowercase && !Regex.IsMatch(password, "[a-z]"))
                validationErrors["Password"].Add(new ValidationErrorDetail("Password must contain at least one lowercase letter."));

            if (_options.RequireUppercase && !Regex.IsMatch(password, "[A-Z]"))
                validationErrors["Password"].Add(new ValidationErrorDetail("Password must contain at least one uppercase letter."));

            if (_options.RequireDigit && !Regex.IsMatch(password, "[0-9]"))
                validationErrors["Password"].Add(new ValidationErrorDetail("Password must contain at least one digit."));

            if (_options.RequireNonAlphanumeric && !Regex.IsMatch(password, @"[\W_]"))
                validationErrors["Password"].Add(new ValidationErrorDetail("Password must contain at least one special character."));


            if (validationErrors["Password"].Count == 0)
                return Result.Success();

            return Result.Validation(validationErrors);
        }

        public Result ValidateCredentials(string email, string password)
        {
            

            var emailResult = ValidateEmail(email);
            var passwordResult = ValidatePassword(password);


            if(emailResult.IsSuccess && passwordResult.IsSuccess)
                return Result.Success();



            var validationErrors = new Dictionary<string, List<ValidationErrorDetail>>();

            if (emailResult.ValidationErrors != null)
                validationErrors["Email"] = emailResult.ValidationErrors.ContainsKey("email") ? emailResult.ValidationErrors["email"] : new List<ValidationErrorDetail>();


            if (passwordResult.ValidationErrors != null)
                validationErrors["Password"] = passwordResult.ValidationErrors.ContainsKey("Password") ? passwordResult.ValidationErrors["Password"] : new List<ValidationErrorDetail>();

                return Result.Validation(validationErrors);
            

        }


        private Result ValidateEmail(string ? Email)
        {


            var validationErrors = new Dictionary<string, List<ValidationErrorDetail>>();
            validationErrors["email"] = new List<ValidationErrorDetail>();


       

            if (string.IsNullOrWhiteSpace(Email))
            {
                validationErrors["email"].Add(new ValidationErrorDetail("Email cannot be empty."));
                return Result.Validation(validationErrors);
            }

            // Simple regex for email format
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(Email, emailPattern))
                validationErrors["email"].Add(new ValidationErrorDetail("Email format is invalid."));

            if (validationErrors["email"].Count == 0)
                return Result.Success();

            return Result.Validation(validationErrors);
        }


    }
    
}
