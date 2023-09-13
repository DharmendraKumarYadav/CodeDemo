using DAL.Core;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace BDB
{
    public class IdentityServerConfig
    {
        public const string ApiName = "bikedekhbike_api";
        public const string ApiFriendlyName = "Bike Dekh Bike API";
        public const string AdminClientID = "bikedekhbike_admin";
        public const string WebClientID = "bikedekhbike_web";
        public const string SwaggerClientID = "swaggerui";

        // Identity resources (used by UserInfo endpoint).
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                new IdentityResources.Email(),
                new IdentityResource(ScopeConstants.Roles, new List<string> { JwtClaimTypes.Role })
            };
        }

        // Api scopes.
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope(ApiName, ApiFriendlyName) {
                    UserClaims = {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Email,
                        JwtClaimTypes.PhoneNumber,
                        JwtClaimTypes.Role
                    }
                }
            };
        }

        // Api resources (Needed for audience to be set on token).
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(ApiName) {
                    Scopes = { ApiName }
                }
            };
        }

        // Clients want to access resources.
        public static IEnumerable<Client> GetClients()
        {
            // Clients credentials.
            return new List<Client>
            {
                new Client
                {
                    ClientId = AdminClientID,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // Resource Owner Password Credential grant.
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false, // This client does not need a secret to request tokens from the token endpoint.
                    
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId, // For UserInfo endpoint.
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Email,
                        ScopeConstants.Roles,
                        ApiName
                    },
                    AllowOfflineAccess = true, // For refresh token.
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AccessTokenLifetime = 180, // Lifetime of access token in seconds.
                    AbsoluteRefreshTokenLifetime = 7200,//7200,
                    SlidingRefreshTokenLifetime = 900,//900
                },
                   new Client
                {
                    ClientId = WebClientID,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // Resource Owner Password Credential grant.
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false, // This client does not need a secret to request tokens from the token endpoint.
                    
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId, // For UserInfo endpoint.
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Phone,
                        IdentityServerConstants.StandardScopes.Email,
                        ScopeConstants.Roles,
                        ApiName
                    },
                    AllowOfflineAccess = true, // For refresh token.
                    RefreshTokenExpiration = TokenExpiration.Sliding,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    AccessTokenLifetime = 86000, // Lifetime of access token in seconds.
                    AbsoluteRefreshTokenLifetime =7200,
                    SlidingRefreshTokenLifetime = 900
                },

                new Client
                {
                    ClientId = SwaggerClientID,
                    ClientName = "Swagger UI",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowAccessTokensViaBrowser = true,
                    RequireClientSecret = false,

                    AllowedScopes = {
                        ApiName
                    }
                }
            };
        }
    }
}
