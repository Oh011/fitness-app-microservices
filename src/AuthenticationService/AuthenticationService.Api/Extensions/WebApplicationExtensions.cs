using AuthenticationService.Infrastructure.Identity.DataSeeding;

namespace AuthenticationService.WebAPI.Extensions
{
    public static class WebApplicationExtensions
    {


        public static async Task<WebApplication> SeedDataBaseAsync(this WebApplication app)
        {



            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var identityDbInitializer = services.GetRequiredService<IIdentityDbInitializer>();
            await identityDbInitializer.InitilaizeAssync();

            return app;

        }

        

    }
}
