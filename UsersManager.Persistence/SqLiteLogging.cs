using System.Data.SQLite;

namespace UsersManager.Persistence;

public static class SqLiteLogging
{
    public static void InitDb(string connectionString)
    {
        using var connection = new SQLiteConnection(connectionString);

        connection.Open();
        const string createTableQuery = """
                                        CREATE TABLE IF NOT EXISTS Log (
                                            TimeStamp TEXT,
                                            Level TEXT,
                                            Logger TEXT,
                                            Message TEXT
                                        );
                                        """;

        using var command = new SQLiteCommand(createTableQuery, connection);
        command.ExecuteNonQuery();
    }

    // ReSharper disable once UnusedMember.Global
    public static async Task LogAsync(string connectionString, string timestamp, string level, string logger,
        string message)
    {
        await using var connection = new SQLiteConnection(connectionString);
        connection.Open();

        await using var command = new SQLiteCommand(connection);
        command.CommandText =
            "INSERT INTO Log(TimeStamp, Level, Logger, Message) VALUES (@Timestamp, @Level, @Logger, @Message)";

        command.Parameters.AddWithValue("@Timestamp", timestamp);
        command.Parameters.AddWithValue("@Level", level);
        command.Parameters.AddWithValue("@Logger", logger);
        command.Parameters.AddWithValue("@Message", message);

        command.ExecuteNonQuery();
    }
}
