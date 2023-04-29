using Duende.IdentityServer.Models;
using IdentityModel;

namespace Authentication.API.Configuration
{
    public class IdentityServer
    {
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new() {
                    Name = "Authentication.read",
                    DisplayName = "Authentication.read",
                    Description = "Authorized to use all GET endpoints on the Service",
                    Required = true,
                    UserClaims = new List<string>
                    {
                        JwtClaimTypes.Email,
                    }
                },

                new() {
                    Name = "Authentication.write",
                    DisplayName = "Authentication.write",
                    Description = "Authorized to use all POST, PUT, PATCH endpoints on the Service",
                    Required = true,
                    UserClaims = new List<string>
                    {
                        JwtClaimTypes.Email,
                    }
                },
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
            {
                new()
                {
                    Name = "Authentication",
                    DisplayName = "Authentication",
                    Scopes = new List<string>
                    {
                        "Authentication.read",
                        "Authentication.write"
                    },
                    UserClaims = new List<string>
                    {
                        JwtClaimTypes.Email
                    }
                }
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new()
                {
                    ClientId = "web",
                    ClientName = "web",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RequireClientSecret = true,
                    AllowedScopes =
                    {
                        "Authentication.read",
                        "Authentication.write"
                    },
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("SuperClientSerctet".Sha256()),
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                }
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
            };
    }
}