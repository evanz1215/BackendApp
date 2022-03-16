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

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetMyOptions<ConnectionStrings>().DefaultConnection,
                    opts => opts.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));

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