using WorkoutService.Application.Abstractions.Data;

namespace WorkoutService.Api.Extensions;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> SeedDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var dbInitializer = services.GetRequiredService<IWorkoutDbInitializer>();
        await dbInitializer.InitializeAsync();

        return app;
    }
}
