using Newtonsoft.Json.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WebAPI.Controllers
{
    public static class ApiStartup
    {
        public static void AddMyApi(this IServiceCollection services)
        {
            services.AddHealthChecks();
            services.AddControllers()
                .AddControllersAsServices()
                //.SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(c =>
                {
                    c.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                })
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    setupAction.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                })
                ;
        }

        /// <summary>
        /// Depends on UseRouting() being called before calling this method.
        /// </summary>
        public static void UseMyApi(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}