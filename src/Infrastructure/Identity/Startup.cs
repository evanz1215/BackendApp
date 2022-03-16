using Infrastructure.Identity.Model;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity
{
    public static class Startup
    {
        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = false;

                options.Password.RequireDigit = false;               //密碼要有數字
                options.Password.RequireLowercase = false;           //要有小寫英文字母
                options.Password.RequireNonAlphanumeric = false;    //不需要符號字元
                options.Password.RequireUppercase = false;           //要有大寫英文字母
                options.Password.RequiredLength = 4;                //密碼至少要6個字元長
                options.Password.RequiredUniqueChars = 1;           //至少要有1個字元不一樣

                options.SignIn.RequireConfirmedEmail = false; //是否需要驗證Email後才能登入
                options.SignIn.RequireConfirmedPhoneNumber = false; //是否需要驗證電話後才能登入
                options.SignIn.RequireConfirmedAccount = false;

                #region 登入錯誤鎖定設定

                //options.Lockout.AllowedForNewUsers = true;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                //options.Lockout.MaxFailedAccessAttempts = 3;

                #endregion 登入錯誤鎖定設定

                //options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub; // JWT specific
            })
                .AddDefaultTokenProviders()

                // Adding Roles is optional, and mostly exists for backwards-compatibility.
                // Not needed if policy/claim based authorization is used (which is recommended).
                // But, if AddRoles() is called, it must be before calling AddEntityFrameworkStores(), because otherwise IRoleStore won't be added (despite what the summary says)..
                //.AddRoles<IdentityRole>()

                .AddEntityFrameworkStores<ApplicationDbContext>(); // EF specific
        }
    }
}