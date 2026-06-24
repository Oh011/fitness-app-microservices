using AuthenticationService.Application.Abstractions.Identity;
using AuthenticationService.Application.Features.Authentication.Dtos.Internal;
using AuthenticationService.Application.Features.Authentication.Dtos.Results;
using Shared.Results;



namespace AuthenticationService.Application.Abstractions.Security.Services
{
    public interface IAuthenticationService
    {  
        Task<Result<long>> RegisterAsync(string Email, string Password, string UserName,string role="User");
        Task<Result<UserDto>> LogInAsync(string Email, string Password, string DeviceId);


    }
}
