
using AuthenticationService.Api.Extensions;
using AuthenticationService.Application.DependecnyInjcetion;
using AuthenticationService.Infrastructure.DependencyInjection;
using AuthenticationService.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace AuthenticationService.Api
{
    public class Program
    {
        public async static Task  Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            builder.Services.AddPresenation();

            builder.Services.AddControllers();


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi


            ;

            var app = builder.Build();

            // Configure the HTTP request pipeline.

                //app.MapOpenApi();



                app.UseSwagger();
                ////app.MapOpenApi();


                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

                app.UseSwaggerUI(options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                                $"Authentication system {description.GroupName.ToUpperInvariant()}");
                    }

                    //options.RoutePrefix = string.Empty; // Swagger at root
                });

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            await app.SeedDataBaseAsync();

            app.Run();
        }
    }
}
