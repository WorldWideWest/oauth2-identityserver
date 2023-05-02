using Authentication.Database;
using Authentication.Models.Configuration;
using Authentication.Models.Entities.Identity;
using Authentication.Models.Interfaces.Services;
using Authentication.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NETCore.MailKit.Extensions;

namespace Authentication.API.Extensions
{
    public static class Services
    {
        public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
        {
            var environment = configuration.GetSection("Application").Get<Application>();
            var connectionString = environment.ConnectionStrings.DefaultConnection;

            services.AddMailKit(config => 
                config.UseMailKit(environment.MailSettings));
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            Identity.Configure(services, configuration);

            services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}