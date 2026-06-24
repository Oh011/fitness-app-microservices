using AuthenticationService.Application.Abstractions.Identity;
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Abstractions.Security.Services;
using AuthenticationService.Application.Common.Errors;
using AuthenticationService.Application.Common.Messages.Success;
using AuthenticationService.Application.Features.Authentication.Dtos.Internal;
using AuthenticationService.Application.Features.Authentication.Dtos.Results;
using AuthenticationService.Infrastructure.Identity.Access;
using AuthenticationService.Infrastructure.Identity.Models;
using AuthenticationService.Infrastructure.Identity.Options;
using AuthenticationService.Infrastructure.Identity.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationService.Infrastructure.Identity.Services.Implementations
{
    internal class TokenService : ITokenService
    {

        private readonly JwtOptions JwtOptions;
        private readonly UserQueryService _queryService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Hasher hasher;


        public TokenService(Hasher hasher, IUnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions, UserQueryService queryService  )
        {
            this.hasher=hasher;

            JwtOptions = jwtOptions.Value;
            this._queryService = queryService;
            _unitOfWork = unitOfWork;
        }

        public async Task<RefreshTokenDto> IssueRefreshTokenForDeviceAsync(long userId, string DeviceId)
        {

            var repository = _unitOfWork.GetRepository<RefreshToken>();

            var OldToken = await repository.FirstOrDefaultAsync(t => t.UserId == userId && t.DeviceId == DeviceId
            && t.IsRevoked == false);

            if (OldToken != null)
            {
                OldToken.Revoke();
            }



            var rawToken = GenerateRefreshToken();

            var NewRefreshToken = new RefreshToken
            {

                UserId = userId,
                TokenHash = hasher.HashWithoutSalt(rawToken),
                ExpiresAt = DateTime.UtcNow.AddDays(14), 
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
                DeviceId = DeviceId,
            };


            await repository.AddAsync(NewRefreshToken);

            await _unitOfWork.Commit();

            return new RefreshTokenDto 
            { Token = rawToken, Expiration = NewRefreshToken.ExpiresAt };
        }







        public async Task<Result> RevokeAllUserSessionsAsync(long userId)
        {
            var repository = _unitOfWork.GetRepository<RefreshToken>();

            var userTokens =await repository.ListAsync(t => t.UserId == userId && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow,asNoTracking:false);

            if (userTokens != null)
            {
                foreach (var token in userTokens)
                {

                    token.IsRevoked = true;
                    token.RevokedAt = DateTime.UtcNow;

                }

                await _unitOfWork.Commit();
            }

            return Result.Success();
        }


        private string GenerateRefreshToken(int size = 64)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(size);

            // Convert to Base64 string
            return Convert.ToBase64String(randomBytes);
        }

        public async Task<string> IssueAccessTokenAsync(long userId, string Email, string UserName)
        {

            var JwtOptions = this.JwtOptions;
            var role = await _queryService.GetRoleNameByUserIdAsync(userId);


            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
                new Claim(ClaimTypes.Email,Email),
                new Claim(ClaimTypes.Name,UserName),
                new Claim(ClaimTypes.Role, role)
    
            };

           


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.SecretKey));


            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var x = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(JwtOptions.ExpirationInHours),
                Issuer = JwtOptions.Issuer,
                Audience = JwtOptions.Audiance,
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }



        // --------------------
        // Shared Validation
        // --------------------
        private async Task<Result<RefreshToken>> GetValidRefreshTokenAsync(string token, string deviceId, bool allowExpired = false)
        {
            var repository = _unitOfWork.GetRepository<RefreshToken>();

            var existingToken = (await repository.FirstOrDefaultAsync(t => t.TokenHash == token && t.DeviceId == deviceId));
     

            if (existingToken == null)
                return Result<RefreshToken>.Unauthorized(AuthErrorMessages.REFRESH_TOKEN_INVALID_OR_EXPIRED);

            if (!allowExpired && (existingToken.IsExpired || existingToken.IsRevoked))
                return Result<RefreshToken>.Unauthorized(AuthErrorMessages.REFRESH_TOKEN_INVALID_OR_EXPIRED);

            return Result<RefreshToken>.Success(existingToken);
        }




        public async Task<Result<LogInUserResponse>> RotateAccessTokenAsync(string refreshToken, string deviceId)
        {
            var hashedToken=hasher.HashWithoutSalt(refreshToken);
            var tokenResult = await  GetValidRefreshTokenAsync(hashedToken, deviceId);

            if (!tokenResult.IsSuccess)
                return Result<LogInUserResponse>.FromResult(tokenResult);

            var storedToken = tokenResult.Value;
            var user = await _queryService.GetByIdAsync(storedToken.UserId);

            if (user == null)
                return Result<LogInUserResponse>.NotFound(AuthErrorMessages.USER_NOT_FOUND);

            // Revoke old refresh token
            storedToken.Revoke();

            var accessToken =await IssueAccessTokenAsync(user.Id, user.Email, user.UserName);
            var newRefreshToken = await IssueRefreshTokenForDeviceAsync(user.Id, deviceId);

          
            var response = new LogInUserResponse(accessToken, newRefreshToken.Token,newRefreshToken.Expiration,
               user.Email, user.Id, user.UserName);

            return Result<LogInUserResponse>.Success(response);
        }

        // --------------------
        // Logout (revoke token)
        // --------------------
        public async Task<Result> RevokeDeviceSessionAsync(string? refreshToken, string deviceId)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return Result.Success(AuthSuccessMessages.LOGOUT_SUCCESS);

            var tokenHash = hasher.HashWithoutSalt(refreshToken);

            var repository = _unitOfWork.GetRepository<RefreshToken>();

            var session = await repository.FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash &&
                     x.DeviceId == deviceId);

            if (session is not null && !session.IsRevoked)
            {
                session.Revoke();
                await _unitOfWork.Commit();
            }


            return Result.Success(AuthSuccessMessages.LOGOUT_SUCCESS);





        }





    }
}


