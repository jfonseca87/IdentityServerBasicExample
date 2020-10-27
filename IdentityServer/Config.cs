using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes = new List<ApiScope>
        {
            new ApiScope("api1", "Product API"),
            new ApiScope("api2", "Product API 2"),
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = 
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes =
                {
                    "api1"
                }
            }
        };
    }
}
