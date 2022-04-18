using Infrastructure.Common.Extensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebAPI.Swagger.Configuration;

namespace WebAPI.Versioning.SwaggerConfiguration
{
    public class ConfigureSwaggerGenOptions : IPostConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _versionProvider;
        private readonly IConfiguration _configuration;

        public ConfigureSwaggerGenOptions(IApiVersionDescriptionProvider versionProvider, IConfiguration configuration)
        {
            _versionProvider = versionProvider;
            _configuration = configuration;
        }


        public void PostConfigure(string _, SwaggerGenOptions options)
        {
            // Clear potentially added unversioned docs.
            options.SwaggerGeneratorOptions.SwaggerDocs.Clear();
            var swaggerSetting = _configuration.GetMyOptions<SwaggerSettings>();

            foreach (var description in _versionProvider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                  description.GroupName,
                    new OpenApiInfo()
                    {
                        //Title = $"{nameof()} {description.ApiVersion}",
                        Title = $"{swaggerSetting.ApiName} {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                    });
            }
        }
    }
}