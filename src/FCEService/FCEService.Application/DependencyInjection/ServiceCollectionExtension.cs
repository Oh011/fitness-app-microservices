using Microsoft.Extensions.DependencyInjection;

namespace FCEService.Application.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFCEApplication(this IServiceCollection services)
    {
        return services;
    }
}
