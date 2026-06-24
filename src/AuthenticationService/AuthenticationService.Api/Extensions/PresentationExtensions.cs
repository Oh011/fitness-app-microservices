
using Authenticore.Api.SwaggerOptions;
using Authenticore.WebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace AuthenticationService.Api.Extensions
{
    public static  class PresentationExtensions
    {


        public static IServiceCollection AddPresenation(this IServiceCollection services)
        {


            services.AddControllers();
            //services.AddEndpointsApiExplorer(); // for minimal APIs & OpenAPI4


            services.AddScoped<CookieService>();

            services.AddControllers()
            .AddJsonOptions(options =>
            {
                //This tells ASP.NET Core to accept and serialize enum values as strings.

                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.AllowTrailingCommas = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;


                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;





            });




            services.AddApiVersioning(opt =>
            {
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.AssumeDefaultVersionWhenUnspecified = true; //--> If a request doesn't specify a version, use the default.
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader(); // use URL path versioning

            });





            ConfigureSwaggerOptions(services);





            return services;
        }



        private static void ConfigureSwaggerOptions(IServiceCollection Services)
        {




           Services.AddSwaggerGen(options =>
            {


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                // Step 1: Define the scheme — same as before
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Description = "Enter your JWT token (no 'Bearer ' prefix needed)"
                });

                // Step 2: NEW delegate-based syntax for .NET 10 / Swashbuckle v10
                options.AddSecurityRequirement(document => new()
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });



            Services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV"; // Format: v1, v2, etc.
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

               

            
            });



            Services.ConfigureOptions<ConfigureSwaggerOptions>();

          

        }
    }
}
