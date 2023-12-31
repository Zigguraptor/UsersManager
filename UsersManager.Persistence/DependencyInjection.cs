﻿using System.Data;
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
        services.AddTransient<IDbConnection>(c =>
            new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
        services.AddTransient<IUsersRepository, UsersRepository>();
        services.AddTransient<IFriendshipRepository, FriendshipRepository>();

        Migrations(configuration);

        return services;
    }

    private static void Migrations(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

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
