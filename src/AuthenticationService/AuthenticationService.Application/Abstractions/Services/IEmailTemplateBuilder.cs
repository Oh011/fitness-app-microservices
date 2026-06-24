using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AuthenticationService.Application.Abstractions.Services
{
    public interface IEmailTemplateBuilder
    {

        public string BuildEmailConfirmationTemplate(string confirmationUrl, string userName);

        public string BuildPasswordResetTemplate(string resetUrl, string userName);

        public string BuildPasswordChangedTemplate(string userName);


    }
}
