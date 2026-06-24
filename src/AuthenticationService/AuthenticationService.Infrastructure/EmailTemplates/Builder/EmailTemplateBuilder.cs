using AuthenticationService.Application.Abstractions.Services;
using AuthenticationService.Infrastructure.Services;
using System.Reflection;


namespace AuthenticationService.Infrastructure.EmailTemplates.Builder
{
    public class EmailTemplateBuilder: IEmailTemplateBuilder
    {
        private readonly Assembly _assembly = typeof(EmailTemplateBuilder).Assembly;

        public string BuildEmailConfirmationTemplate(string confirmationUrl, string userName)
        {
            var template = LoadTemplate("EmailConfirmation.html");
            return template
                .Replace("{{ConfirmationUrl}}", confirmationUrl)
                .Replace("{{UserName}}", userName);
        }

        public string BuildPasswordResetTemplate(string resetUrl, string userName)
        {
            var template = LoadTemplate("PasswordReset.html");
            return template
                .Replace("{{ResetUrl}}", resetUrl)
                .Replace("{{UserName}}", userName);
        }

        public string BuildPasswordChangedTemplate(string userName)
        {
            var template = LoadTemplate("PasswordChanged.html");
            return template
                .Replace("{{UserName}}", userName);
        }

        private string LoadTemplate(string fileName)
        {
            var resourceName = $"{_assembly.GetName().Name}.EmailTemplates.{fileName}";
            using var stream = _assembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Embedded template not found: {resourceName}");
            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
