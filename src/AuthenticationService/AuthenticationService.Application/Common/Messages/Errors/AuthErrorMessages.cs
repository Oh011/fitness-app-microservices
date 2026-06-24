using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Common.Errors
{

    public static class AuthErrorMessages
    {
        public const string USER_ALREADY_EXISTS = "A user with this email already exists.";
        public const string INVALID_CREDENTIALS = "Invalid email or password.";
        public const string ACCOUNT_LOCKED = "This account has been locked.";
        public const string EMAIL_NOT_CONFIRMED = "Email is not confirmed.";
        public const string PASSWORD_RESET_FAILED = "Password reset failed.";
        public const string TOKEN_INVALID_OR_EXPIRED = "Authentication token is invalid or expired.";
        public const string REFRESH_TOKEN_INVALID_OR_EXPIRED = "Invalid or expired refresh token.";
        public const string REFRESH_TOKEN_NOT_FOUND = "Refresh token not found for this user/device.";
        public const string REFRESH_TOKEN_ALREADY_REVOKED = "Refresh token is already revoked.";
        public const string USER_NOT_FOUND = "User not found.";


        public const string INVALID_OR_EXPIRED_RESET_CODE = "Invalid or expired reset code.";

        // Email confirmation (OTP-based)
        public const string EMAIL_ALREADY_CONFIRMED = "Email is already confirmed.";
        public const string INVALID_OR_EXPIRED_CONFIRMATION_CODE = "Invalid or expired confirmation code.";
        public const string TOO_MANY_CONFIRMATION_ATTEMPTS = "Too many incorrect attempts. Please request a new code.";
        public const string CONFIRMATION_CODE_EXPIRED = "Confirmation code has expired.";
    }

}
