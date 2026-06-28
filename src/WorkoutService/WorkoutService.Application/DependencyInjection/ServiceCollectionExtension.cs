using Microsoft.Extensions.DependencyInjection;

namespace WorkoutService.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddWorkoutApplication(this IServiceCollection services)
    {
        // Application layer has no implementations to register.
        // Service implementations are registered in the Infrastructure layer.
        return services;
    }
}
