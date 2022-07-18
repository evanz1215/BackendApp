using Infrastructure.Common.Extensions;
using WebAPI.CORS.Settings;

namespace WebAPI.CORS
{
    public static class CorsStartup
    {
        public static void AddMyCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var corsSettings = configuration.GetMyOptions<CorsSettings>();

            if (corsSettings == null)
                return;

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    var corsPolicy = builder
                    .AllowAnyMethod()
                    .AllowAnyHeader();

                    if (corsSettings != null && corsSettings.AllowedOrigins.Length > 0)
                    {
                        corsPolicy.AllowCredentials();
                        corsPolicy.SetIsOriginAllowedToAllowWildcardSubdomains();
                        corsPolicy.WithOrigins(
                        corsSettings.AllowedOrigins);
                    }
                    else
                    {
                        corsPolicy.AllowAnyOrigin();
                    }
                    corsPolicy.Build();
                });
            });
        }

        public static void UseMyCorsConfiguration(this IApplicationBuilder app)
        {
            app.UseCors();
        }
    }
}