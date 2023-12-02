using System.Data;
using System.Reflection;
using DbUp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using UsersManager.Application.Interfaces;

namespace UsersManager.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection
        services, IConfiguration configuration)
    {
        var connectionString = Environment.GetEnvironmentVariable("PG_CONNECTION_STRING");
        connectionString ??= configuration.GetConnectionString("DefaultConnection");
        if (connectionString == null)
            throw new NullReferenceException(
                """
                Не найдена строка подключения к PG.
                Необходимо указать, либо PG_CONNECTION_STRING в enviroment variables,
                либо DefaultConnection в appsettings.json.
                """);

        services.AddTransient<IDbConnection>(c => new NpgsqlConnection(connectionString));
        services.AddTransient<IUsersRepository, UsersRepository>();
        services.AddTransient<IFriendshipRepository, FriendshipRepository>();

        Migrations(connectionString);

        return services;
    }

    private static IDbConnection TryGetDbConnection(string connectionString)
    {
        var maxAttempts = 100;
        IDbConnection? connection = null;

        for (var i = 0; i < maxAttempts; i++)
        {
            try
            {
                connection = new NpgsqlConnection(connectionString);
                connection.Open();
                if (connection.State == ConnectionState.Open) break;
            }
            catch
            {
                connection = null;
                Thread.Sleep(1000);
            }
        }

        if (connection == null)
            throw new Exception("Не удалось подключиться к БД.");
        
        return connection;
    }

    private static void Migrations(string connectionString)
    {
        TryGetDbConnection(connectionString).Dispose();
        
        EnsureDatabase.For.PostgresqlDatabase(connectionString);

        var upgrade =
            DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

        if (upgrade.IsUpgradeRequired())
        {
            var result = upgrade.PerformUpgrade();
            if (!result.Successful)
                throw new Exception("Не удалось выполнить миграции.");

            Console.WriteLine("База данных успешно обновлена до последней версии.");
        }
    }
}
