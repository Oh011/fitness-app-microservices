using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationService.Application.Common.Messages.Success
{
    public static class AuthSuccessMessages
    {
        public const string USER_REGISTERED = "User registered successfully. Please confirm your email.";
        public const string EMAIL_CONFIRMATION_SENT = "Email confirmation link has been sent.";
        public const string EMAIL_CONFIRMED = "Email confirmed successfully.";

        // New message for email already confirmed
        public const string PASSWORD_RESET_EMAIL_SENT_IF_EXISTS = "If an account with that email exists, a password reset link has been sent.";
        public const string EMAIL_ALREADY_CONFIRMED = "Email is already confirmed.";

        public const string LOGIN_SUCCESS = "Login successful.";
        public const string LOGOUT_SUCCESS = "Logged out successfully.";
        public const string PASSWORD_RESET_EMAIL_SENT = "If an account with that email exists, a password reset link has been sent.";
        public const string PASSWORD_RESET_SUCCESS = "Password has been reset successfully.";
        public const string PASSWORD_CHANGED = "Password changed successfully.";
        public const string TOKEN_REFRESHED = "Token refreshed successfully.";
    }

}
