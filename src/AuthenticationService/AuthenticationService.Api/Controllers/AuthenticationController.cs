
using AuthenticationService.Api.Extensions;
using AuthenticationService.Application.Features.Authentication.Commands;
using AuthenticationService.Application.Features.Authentication.Dtos.Results;
using Authenticore.WebAPI.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Responses;
using System.Net;

namespace AuthenticationService.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController(IMediator mediator, CookieService cookieService) : ControllerBase
    {
        /// <summary>
        /// Registers a new user account and sends an email confirmation link.
        /// </summary>
        /// <remarks>Anonymous. Creates the identity user (and domain user via orchestration) and queues a confirmation email.</remarks>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> Register(RegisterUserCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToApiResponse();
        }

        /// <summary>
        /// Authenticates a user with email and password, returning an access token and setting a refresh token cookie.
        /// </summary>
        /// <remarks>Anonymous. On success, sets an HttpOnly refresh token cookie and returns the access token in the response body.</remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> Login(LoginUserCommand command)
        {
            var result = await mediator.Send(command);

            if (result.IsSuccess && result.Value is not null)
            {
                cookieService.SetRefreshToken(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);
                return Ok(ApiResponse< AuthenticationResponse>.Ok( new AuthenticationResponse(
                    result.Value.Email, result.Value.UserId, result.Value.UserName, 
                    result.Value.AccessToken),result.Message));
            }

            return result.ToApiResponse();
        }

        /// <summary>
        /// Exchanges a valid refresh token for a new access token, rotating the refresh token.
        /// </summary>
        /// <remarks>Anonymous. Refresh token is read from the HttpOnly cookie if present, otherwise from the request body (mobile clients).</remarks>
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> RefreshToken([FromBody] RefreshAccessTokenCommand request)
        {
            var refreshToken = Request.Cookies["refreshToken"] ?? request.RefreshToken;

            if (string.IsNullOrEmpty(refreshToken))
            {
                if (request.RefreshToken == null)
                    return Unauthorized(ApiResponse.Unauthorized("UnAuthorized"));

                refreshToken = request.RefreshToken;
            }

            request.RefreshToken = refreshToken;

            var result = await mediator.Send(request);

            if (result.IsSuccess && result.Value is not null)
            {
                cookieService.SetRefreshToken(result.Value.RefreshToken, result.Value.RefreshTokenExpiration);
                return Ok(ApiResponse<AuthenticationResponse>.Ok(new AuthenticationResponse(
                    result.Value.Email, result.Value.UserId, result.Value.UserName, result.Value.AccessToken),
                    result.Message));
            }

            return result.ToApiResponse();
        }

        /// <summary>
        /// Confirms a user's email address using the token sent via the confirmation email link.
        /// </summary>
        /// <remarks>Anonymous. GET because the link is clicked directly from the email client.</remarks>
        [HttpPost("confirm-email")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToApiResponse();

        }

        /// <summary>
        /// Resends the email confirmation link to the specified email address.
        /// </summary>
        /// <remarks>Anonymous. Rate-limited per account to prevent mail-bombing.</remarks>
        [HttpPost("resend-confirmation")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ResendConfirmEmail(ResendConfirmationEmailCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToApiResponse();

        }

        /// <summary>
        /// Sends a password reset link to the given email if an account exists.
        /// </summary>
        /// <remarks>Anonymous. Always returns a generic success response, regardless of whether the email exists, to prevent account enumeration.</remarks>
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ForgotPassword(ForgotPasswordCommand command)
        {
            var result = await mediator.Send(command);
            return result.ToApiResponse();
            

           
        }

        /// <summary>
        /// Resets a user's password using a valid reset token from the password reset email.
        /// </summary>
        /// <remarks>Anonymous. Consumes the reset token, updates the password hash, and revokes all existing sessions.</remarks>
        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse>> ResetPassword(ResetPasswordCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToApiResponse();
          
        }

        /// <summary>
        /// Changes the password for the currently authenticated user.
        /// </summary>
        /// <remarks>Requires authentication. Verifies the old password before updating to the new one.</remarks>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> ChangePassword(ChangePasswordCommand command)
        {
            var result = await mediator.Send(command);

            return result.ToApiResponse();
        }



        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> LogOut (LogOutCommanad request)
        {
            var refreshToken = Request.Cookies["refreshToken"] ?? request.RefreshToken;

            var result= await mediator.Send(request);
 

            cookieService.RemoveRefreshToken();


            return result.ToApiResponse();


        }
    }

}
