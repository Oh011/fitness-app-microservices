using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkoutService.Application.Abstractions.Data;
using WorkoutService.Application.Abstractions.Services;
using WorkoutService.Infrastructure.DataSeeding;
using WorkoutService.Infrastructure.Persistence.Context;
using WorkoutService.Infrastructure.Services;

namespace WorkoutService.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddWorkoutInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("WorkoutDbConnection")));

        services.AddScoped<IWorkoutService, WorkoutService.Infrastructure.Services.WorkoutService>();
        services.AddScoped<IExerciseService, ExerciseService>();
        services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();
        services.AddScoped<IWorkoutDbInitializer, WorkoutDbInitializer>();

        return services;
    }
}
