using System.Text;
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
            builderConfiguration.Bind("TokenValidationParameters", options.TokenValidationParameters);

            var securityKey = Encoding.UTF8.GetBytes(builderConfiguration["TokenValidationSecurityKey"] ??
                                                     throw new InvalidOperationException());
            options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(securityKey);
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireAdministratorRole",
                policyBuilder => policyBuilder.RequireRole("Administrator"));
        });

        return services;
    }
}
