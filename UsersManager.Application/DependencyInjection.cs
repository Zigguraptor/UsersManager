using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using UsersManager.Application.Interfaces;
using UsersManager.Application.Security;

namespace UsersManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddSingleton<IPasswordHandler, BCryptPasswordHandler>();
        services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();

        return services;
    }
}
