using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NutritionService.Application.Abstractions.Data;
using NutritionService.Application.Abstractions.Services;
using NutritionService.Infrastructure.DataSeeding;
using NutritionService.Infrastructure.Persistence.Context;
using NutritionService.Infrastructure.Services;

namespace NutritionService.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddNutritionInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<NutritionDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));

        services.AddScoped<INutritionService, NutritionService.Infrastructure.Services.NutritionService>();
        services.AddScoped<INutritionDbInitializer, NutritionDbInitializer>();

        services.AddHttpClient<IFceServiceClient, FceServiceClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["FceApi:BaseUrl"] ?? "http://fce-service:8080");
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        return services;
    }
}
