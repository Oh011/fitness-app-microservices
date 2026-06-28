using WorkoutService.Api.Extensions;
using WorkoutService.Application.DependencyInjection;
using WorkoutService.Infrastructure.DependencyInjection;

namespace WorkoutService.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add layers
        builder.Services.AddWorkoutInfrastructure(builder.Configuration);
        builder.Services.AddWorkoutApplication();
        builder.Services.AddWorkoutPresentation(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        await app.SeedDatabaseAsync();

        app.Run();
    }
}
