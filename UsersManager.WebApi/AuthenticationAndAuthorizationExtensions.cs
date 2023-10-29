using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace UsersManager.WebApi;

public static class AuthenticationAndAuthorizationExtensions
{
    public static IServiceCollection AddCustomAuthenticationAndAuthorization(this IServiceCollection services,
        ConfigurationManager builderConfiguration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                RequireExpirationTime = false,
                RequireSignedTokens = true,
                RequireAudience = false,
                SaveSigninToken = false,
                TryAllIssuerSigningKeys = true,
                ValidateActor = false,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                ValidateTokenReplay = false,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey("eb58ed4d-46e3-46a5-ab25-55cb1412f1e6"u8.ToArray())
            };
        });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdministratorRole",
                policyBuilder => policyBuilder.RequireRole("Administrator"));
        });

        return services;
    }
}
