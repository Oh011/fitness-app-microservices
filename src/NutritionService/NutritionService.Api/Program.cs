using NutritionService.Api.Extensions;
using NutritionService.Application.DependencyInjection;
using NutritionService.Infrastructure.DependencyInjection;

namespace NutritionService.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddNutritionInfrastructure(builder.Configuration);
        builder.Services.AddNutritionApplication();
        builder.Services.AddNutritionPresentation(builder.Configuration);

        var app = builder.Build();

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
