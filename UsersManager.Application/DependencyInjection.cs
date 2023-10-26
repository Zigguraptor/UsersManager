using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace UsersManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
