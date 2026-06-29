using FCEService.Api.Extensions;
using FCEService.Application.DependencyInjection;
using FCEService.Infrastructure.DependencyInjection;

namespace FCEService.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddFCEInfrastructure(builder.Configuration);
            builder.Services.AddFCEApplication();
            builder.Services.AddFCEPresentation(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.SeedDatabaseAsync();
            app.Run();
        }
    }
}
