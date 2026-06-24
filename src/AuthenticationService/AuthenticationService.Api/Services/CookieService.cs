namespace Authenticore.WebAPI.Services
{
    public class CookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor accessor)
        {
            _httpContextAccessor = accessor;
        }

        public void SetRefreshToken(string token, DateTime expires)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = expires
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", token, options);
        }


        public void RemoveRefreshToken()
        {


            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
        }
    }

}
