using Authentication.API.Configuration;
using Authentication.API.Providers;
using Authentication.Database;
using Authentication.Models.Configuration;
using Authentication.Models.Entities.Identity;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityModel.Client;
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

                options.Tokens.EmailConfirmationTokenProvider = "emailconfirmation";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailconfirmation")
            .AddDefaultTokenProviders();

            services.Configure<EmailConfirmationTokenProviderOptions>(options =>
                options.TokenLifespan = TimeSpan.FromDays(2));

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

            services.AddSingleton<IDiscoveryCache>(cache =>
            {
                var factory = cache.GetRequiredService<IHttpClientFactory>();

                return new DiscoveryCache(
                    environment.AuthUri, () => factory.CreateClient());
            });
            
            return services;
        }

        public static void EnsureSeedData(WebApplication app, IConfiguration configuration)
        {
            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in IdentityServer.Clients)
                    context.Clients.Add(client.ToEntity());
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in IdentityServer.IdentityResources)
                    context.IdentityResources.Add(resource.ToEntity());
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var apiScope in IdentityServer.ApiScopes)
                    context.ApiScopes.Add(apiScope.ToEntity());
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in IdentityServer.ApiResources)
                    context.ApiResources.Add(resource.ToEntity());
                context.SaveChanges();
            }
        }
    }
}