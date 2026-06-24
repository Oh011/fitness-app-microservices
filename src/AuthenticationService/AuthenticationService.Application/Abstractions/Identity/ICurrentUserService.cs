using System;
using System.Collections.Generic;
using System.Text;

namespace AuthenticationService.Application.Abstractions.Identity
{




    public interface ICurrentUserService
    {
        long ? UserId { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
    }
}
