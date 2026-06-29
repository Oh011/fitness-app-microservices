using FCEService.Application.Abstractions;
using FCEService.Infrastructure.DataSeeding;
using FCEService.Infrastructure.Persistence.Context;
using FCEService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FCEService.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFCEInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<FCEDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("FCEDbConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

        services.AddScoped<IFitnessService, FitnessService>();
        services.AddScoped<IFCEDbInitializer, FCEDbInitializer>();

        return services;
    }
}
