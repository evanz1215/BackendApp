using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.Versioning.SwaggerConfiguration
{
    public class ConfigureSwaggerGenOptions : IPostConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _versionProvider;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider versionProvider)
            => _versionProvider = versionProvider;

        public void PostConfigure(string _, SwaggerGenOptions options)
        {
            // Clear potentially added unversioned docs.
            options.SwaggerGeneratorOptions.SwaggerDocs.Clear();

            foreach (var description in _versionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                  description.GroupName,
                    new OpenApiInfo()
                    {
                        //Title = $"{nameof()} {description.ApiVersion}",
                        Title = $"{description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                    });
            }
        }
    }
}