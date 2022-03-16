using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebAPI.Versioning.SwaggerConfiguration;

namespace WebAPI.Versioning
{
    public static class VersioningStartup
    {
        public static void AddMyVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);

                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            // If Swagger is enabled, add versioning support to it.
            if (services.Any(x => x.ServiceType == typeof(ISwaggerProvider)))
            {
                services.AddVersionedApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                    options.AddApiVersionParametersWhenVersionNeutral = true;
                });

                // Override Swagger configurations with versioned ones.
                services.AddTransient<IPostConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
                services.AddTransient<IPostConfigureOptions<SwaggerUIOptions>, ConfigureSwaggerUIOptions>();
            }
        }
    }
}