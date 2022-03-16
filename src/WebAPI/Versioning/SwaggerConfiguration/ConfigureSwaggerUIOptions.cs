using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace WebAPI.Versioning.SwaggerConfiguration
{
    public class ConfigureSwaggerUIOptions : IPostConfigureOptions<SwaggerUIOptions>
    {
        private readonly IApiVersionDescriptionProvider _versionProvider;

        public ConfigureSwaggerUIOptions(IApiVersionDescriptionProvider versionProvider)
            => _versionProvider = versionProvider;

        public void PostConfigure(string _, SwaggerUIOptions options)
        {
            // Clear potentially added unversioned endpoints.
            options.ConfigObject.Urls = Enumerable.Empty<UrlDescriptor>();

            foreach (var description in _versionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    url: $"/swagger/{description.GroupName}/swagger.json",
                    name: description.GroupName.ToUpperInvariant());
            }
        }
    }
}