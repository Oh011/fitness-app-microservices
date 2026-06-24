
using AuthenticationService.Application.Abstractions.Bcakground;
using AuthenticationService.Application.Abstractions.Identity;
using AuthenticationService.Application.Abstractions.Messaging.Email;
using AuthenticationService.Application.Abstractions.Persistence;
using AuthenticationService.Application.Abstractions.Security.Services;
using AuthenticationService.Application.Abstractions.Services;
using AuthenticationService.Infrastructure.Background;
using AuthenticationService.Infrastructure.EmailTemplates.Builder;
using AuthenticationService.Infrastructure.Identity.Access;
using AuthenticationService.Infrastructure.Identity.DataSeeding;
using AuthenticationService.Infrastructure.Identity.Options;
using AuthenticationService.Infrastructure.Identity.Security;
using AuthenticationService.Infrastructure.Identity.Services.Implementations;
using AuthenticationService.Infrastructure.Messaging.Email;
using AuthenticationService.Infrastructure.Persistence.Context;
using AuthenticationService.Infrastructure.Persistence.Repositories;
using AuthenticationService.Infrastructure.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


using authenticationService = AuthenticationService.Infrastructure.Identity.Services.Implementations.AuthenticationService;

namespace AuthenticationService.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, IConfiguration configuration)
        {
          
      

            services.AddScoped<IdentityCommandService>();
            services.AddScoped<UserQueryService>();
            services.AddSingleton<Hasher>();
            services.AddSingleton<CredentialValidator>();


            services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));

            services.AddHttpContextAccessor();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEmailTemplateBuilder, EmailTemplateBuilder>();
            services.AddScoped<IUrlBuilder,UrlBuilder>();
            services.AddScoped<IBackgroundJobQueue, HangfireBackgroundJobQueue>();




            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

          

                services.AddHangfire(config =>
                {
                    config.UseSqlServerStorage(configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
                });


            services.AddHangfireServer();


            services.Configure<SmtpOptions>(configuration.GetSection("SmtpOptions"));


            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.AddScoped<IAuthenticationService, authenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailVerificationService, EmailVerificationService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IRoleService, RoleService>();


            services.AddScoped<IEmailService, EmailService>();


            services.AddDbContext<ApplicationDbContext>(options =>
            {

                options.UseSqlServer(configuration.GetSection("ConnectionStrings")["DefaultConnection"]);
            });


            services.Configure<PasswordOptions>(
                configuration.GetSection("PasswordOptions"));

            services.AddScoped<IIdentityDbInitializer, IdentityDbInitializer>();


            ConfigureJwtOptions(services, configuration);

            return services;
        }


        private static void ConfigureJwtOptions(this IServiceCollection services, IConfiguration configuration)
        {

            var jwtOptions = configuration.GetSection("JwtOptions").Get<JwtOptions>();


            services.AddAuthentication(options =>
            {

                //{How to check if a user is logged in}
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //{How to respond to unauthenticated requests}
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            }

                ).AddJwtBearer(options =>
                {




                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            // prevent redirect to login page
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = System.Text.Json.JsonSerializer.Serialize(new { message = "Unauthorized yes or no" });
                            return context.Response.WriteAsync(result);
                        },


                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our SignalR hub
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/hubs/notifications")) // make sure this matches your actual hub route
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };


                    options.TokenValidationParameters = new TokenValidationParameters
                    {

                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,


                        RoleClaimType = ClaimTypes.Role,


                        ValidAudience = jwtOptions.Audiance,

                        ValidIssuer = jwtOptions.Issuer,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                        ClockSkew = TimeSpan.Zero,
                    };
                }





                );
        }

    }
}
