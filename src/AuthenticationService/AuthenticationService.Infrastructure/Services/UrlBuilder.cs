using AuthenticationService.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Infrastructure.Services
{
    internal class UrlBuilder : IUrlBuilder
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UrlBuilder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private string BaseUrl
        {
            get
            {
                var request = _httpContextAccessor.HttpContext?.Request
                    ?? throw new InvalidOperationException("No active HTTP request.");

                return $"{request.Scheme}://{request.Host}";
            }
        }

        public string BuildEmailConfirmationUrl(string rawToken)
        {
            return $"{BaseUrl}/api/auth/confirm-email?token={Uri.EscapeDataString(rawToken)}";
        }

        public string BuildPasswordResetUrl(string rawToken)
        {
            return $"{BaseUrl}/api/auth/reset-password?token={Uri.EscapeDataString(rawToken)}";
        }
    }
}
