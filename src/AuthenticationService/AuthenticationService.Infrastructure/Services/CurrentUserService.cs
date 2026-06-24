using AuthenticationService.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AuthenticationService.Infrastructure.Services
{
    internal class CurrentUserService : ICurrentUserService
    {

        private readonly IHttpContextAccessor _httpContextAccessor; 
        

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public long? UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.NameIdentifier)?
                    .Value;

                if (value is null) return null;

                return long.TryParse(value, out var id) ? id : null;
            }
        }


        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        public bool IsAuthenticated =>
              _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
