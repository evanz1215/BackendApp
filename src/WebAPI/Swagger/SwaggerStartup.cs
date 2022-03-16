using Infrastructure.Common.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using WebAPI.Swagger.Configuration;

namespace WebAPI.Swagger
{
    public static class SwaggerStartup
    {
        public static void AddMySwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetMyOptions<SwaggerSettings>();

            if (!swaggerSettings.UseSwagger)
            {
                return;
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerSettings.ApiName, Version = "v1" });

                c.AddSecurityDefinition(SecuritySchemeNames.Bearer, new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddTransient<IConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();
        }

        public static void UseMySwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetMyOptions<SwaggerSettings>();

            if (swaggerSettings.UseSwagger == true)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
        }
    }
}