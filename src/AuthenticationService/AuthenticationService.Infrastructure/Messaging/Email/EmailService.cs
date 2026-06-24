using AuthenticationService.Application.Abstractions.Messaging.Email;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Text;
using MailKit.Security;
using AuthenticationService.Infrastructure.Identity.Options;
using Shared.Results;
using Shared;

namespace AuthenticationService.Infrastructure.Messaging.Email
{
    internal class EmailService : IEmailService
    {


        private readonly SmtpOptions smtpOptions;
        private ILogger<IEmailService> _logger;


        public EmailService(IOptions<SmtpOptions> smtpOptions, ILogger<IEmailService> logger)
        {
            this.smtpOptions = smtpOptions.Value;
            _logger = logger;
        }


        public async Task<Result> SendEmailAsync(EmailMessage message)
        {

            try
            {

                var EmailMessage = new MimeMessage();


                EmailMessage.From.Add(new MailboxAddress(smtpOptions.DisplayName, smtpOptions.FromAddress));

                EmailMessage.To.Add(MailboxAddress.Parse(message.To));

                EmailMessage.Subject = message.Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = message.IsHtml ? message.Body : null,
                    TextBody = message.IsHtml ? null : message.Body
                };


                foreach (var attachement in message.Attachments)
                {
                    builder.Attachments.Add(attachement.FileName, attachement.Content, ContentType.Parse(attachement.ContentType));
                }

                EmailMessage.Body = builder.ToMessageBody();


                using var clinet = new SmtpClient();
                await clinet.ConnectAsync(smtpOptions.Host, smtpOptions.Port, SecureSocketOptions.StartTls);
                await clinet.AuthenticateAsync(smtpOptions.Username, smtpOptions.Password);
                await clinet.SendAsync(EmailMessage);
                await clinet.DisconnectAsync(true);


            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while sending email to {Email}", message.To);


                return Result.Infrastructure("Email sending failed");
        
            }


            return Result.Success();
        }
    }
}
