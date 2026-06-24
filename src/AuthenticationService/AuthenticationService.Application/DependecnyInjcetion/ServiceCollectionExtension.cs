using AuthenticationService.Application.Features;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using AuthenticationService.Application.Common.Behaviors;


namespace AuthenticationService.Application.DependecnyInjcetion
{
    public static class ServiceCollectionExtension
    {


        public static IServiceCollection AddApplication(
        this IServiceCollection services)
        {






            services.AddMediatR(T =>
            {

                T.RegisterServicesFromAssembly(typeof(MediatRAssemblyReference).Assembly);
            });

            services.AddValidatorsFromAssembly(typeof(Application.Common.AssemblyReference).Assembly);




            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));




            return services;



        }


        }
}
