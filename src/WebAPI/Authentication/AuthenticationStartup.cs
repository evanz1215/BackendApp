using Application.Common.Dependencies.Services;
using WebAPI.Authentication.Services;

namespace WebAPI.Authentication
{
    public static class AuthenticationStartup
    {
        public static void AddMyApiAuthDeps(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
        }
    }
}