using Authentication.Database;
using Authentication.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Authentication.API.Extensions
{
    public static class Identity
    {
        public static IServiceCollection Configure(IServiceCollection services, IConfiguration configuration)
        {

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                
                options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
    }
}