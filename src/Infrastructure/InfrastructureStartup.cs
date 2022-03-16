using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure
{
    public static class InfrastructureStartup
    {
        public static void AddMyInfrastructureConfiguration(this IConfigurationBuilder configBuilder, HostBuilderContext context)
        {
            //configBuilder.AddJsonFile("infrastructureSettings.json", optional: true);
        }

        public static void AddMyInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            Identity.Startup.ConfigureServices(services, configuration);
            Authentication.Startup.ConfigureServices(services, configuration);
            Persistence.Startup.ConfigureServices(services, configuration, env);
            ApplicationDependencies.Startup.ConfigureServices(services, configuration);
        }

        public static void UseMyInfrastructure(this IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment env)
        {
            Authentication.Startup.Configure(app);
            Persistence.Startup.Configure(app, configuration);
        }
    }
}