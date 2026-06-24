
using Shared;
using Shared.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Abstractions.Messaging.Email
{
    public interface IEmailService
    {


      public  Task<Result> SendEmailAsync(EmailMessage emailMessage);
    }
}
