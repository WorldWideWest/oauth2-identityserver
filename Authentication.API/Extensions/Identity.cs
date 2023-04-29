using Authentication.Database;
using Authentication.Models.Configuration;
using Authentication.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication.API.Extensions
{
    public static class Identity
    {
        public static IServiceCollection Configure(IServiceCollection services, IConfiguration configuration)
        {
            var environment = configuration.GetSection("Application").Get<Application>();

            var connectionString = environment.ConnectionStrings.DefaultConnection;
            var migrationAssembly = typeof(ApplicationDbContext).Assembly.GetName().Name;

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

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = context => context.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = context => context.UseSqlServer(connectionString,
                    sql => sql.MigrationsAssembly(migrationAssembly));
                options.EnableTokenCleanup = true;
            })
            .AddAspNetIdentity<User>();
            
            return services;
        }
    }
}