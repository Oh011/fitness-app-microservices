
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService. Application.Abstractions.Security.Services
{
    public interface IEmailVerificationService
    {

        Task<Result> SendConfirmationEmail(string email);

        Task<Result> ConfirmEmailAsync(string token);


    }
}
