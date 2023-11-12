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
}
