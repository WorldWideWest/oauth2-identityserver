using Authentication.Database;
using Authentication.Models.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Extensions
{
    public static class Services
    {
        public static IServiceCollection Add(this IServiceCollection services, IConfiguration configuration)
        {
            var environment = configuration.GetSection("Application").Get<Application>();
            var connectionString = environment.ConnectionStrings.DefaultConnection;
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            Identity.Configure(services, configuration);


            return services;
        }
    }
}