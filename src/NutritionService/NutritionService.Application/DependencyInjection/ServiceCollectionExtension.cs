using Microsoft.Extensions.DependencyInjection;

namespace NutritionService.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddNutritionApplication(this IServiceCollection services)
    {
        return services;
    }
}
