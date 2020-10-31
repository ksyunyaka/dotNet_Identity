using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VM.Ident.Identity
{
    public class Config
    {
        public static IEnumerable<IdentityResource> Ids =>
             new IdentityResource[]
             {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    UserClaims = { JwtClaimTypes.Role }
                }
             };


        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                // the api requires the role claim
                new ApiResource("weatherapi", "The Weather API", new[] { JwtClaimTypes.Role })
            };


        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    ClientId = "blazor",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = { "https://localhost:5001" },
                    AllowedScopes = { "openid", "email", "weatherapi","roles" },
                    RedirectUris = { "https://localhost:5001/authentication/login-callback" },
                    PostLogoutRedirectUris = { "https://localhost:5001/" },
                    Enabled = true
                },
                 new Client
                {
                    ClientId = "postman",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = true,
                    ClientSecrets = {new Secret("aloha".Sha256())},
                    AllowedScopes = { "openid", "email", "weatherapi", "role"},
                    Enabled = true
                },
            };
    }
}