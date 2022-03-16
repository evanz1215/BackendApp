using System.ComponentModel.DataAnnotations;

namespace WebAPI.Swagger.Configuration
{
    public class SwaggerSettings
    {
        [Required, MinLength(1)]
        public string ApiName { get; init; }

        public bool UseSwagger { get; init; }

        [Required, MinLength(1)]
        public string LoginPath { get; set; }
    }
}