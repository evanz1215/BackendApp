using Infrastructure.Common.Extensions;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Persistence
{
    public static class Startup
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration, IHostEnvironment env)
        {
            services.AddSingleton(configuration.GetMyOptions<ApplicationDbSettings>());
            var settings = configuration.GetMyOptions<ApplicationDbSettings>();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                switch (settings.Provider.ToUpper())
                {
                    case "MSSQL":
                    default:
                        options.UseSqlServer(
                configuration.GetMyOptions<ConnectionStrings>().DefaultConnection,
                opts => opts.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                        break;

                    case "MYSQL":
                        options.UseMySql(
                            configuration.GetMyOptions<ConnectionStrings>().DefaultConnection,
                    MySqlServerVersion.LatestSupportedServerVersion);
                        break;
                }

                // Allows log messages to contain the normally masked
                // SQL statement parameters sent to DB.
                if (env.IsDevelopment())
                    options.EnableSensitiveDataLogging();
            });
        }

        public static void Configure(IApplicationBuilder app, IConfiguration configuration)
        {
            Seed.DbInitializer.SeedDatabase(app, configuration);
        }
    }
}