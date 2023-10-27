using System.Data;
using Dapper;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Interfaces;
using UsersManager.Domain;

namespace UsersManager.Persistence;

public class UsersRepository : IUsersRepository
{
    private readonly IDbConnection _dbConnection;

    public UsersRepository(IDbConnection dbConnection) => _dbConnection = dbConnection;

    public Task<User?> GetUserAsync(Guid? userGuid = null, string? userName = null, string? emailAddress = null)
    {
        if (userGuid == null && userName == null && emailAddress == null)
            throw new ArgumentException();

        userName = userName?.ToLower();
        emailAddress = emailAddress?.ToLower();

        const string sql = """
                           SELECT *
                           FROM "public"."Users"
                           WHERE "IsActive"
                           AND ("Uuid" = @userGuid OR @userGuid IS NULL)
                           AND (lower("UserName") = @userName OR @userName IS NULL)
                           AND (lower("EmailAddress") = @emailAddress OR @emailAddress IS NULL);
                           """;

        return _dbConnection.QueryFirstOrDefaultAsync<User>(sql,
            new { userGuid, userName, emailAddress });
    }

    public Task<Guid> InsertUserAsync(User user)
    {
        const string sql = """
                           INSERT INTO "public"."Users" ("UserName", "DisplayName", "EmailAddress")
                           VALUES (@UserName, @DisplayName, @EmailAddress)
                           RETURNING "Uuid";
                           """;
        return _dbConnection.QuerySingleAsync<Guid>(sql, user);
    }

    public async Task<int> UpdateUserAsync(User user)
    {
        const string sql = """
                           UPDATE "public"."Users"
                           SET ("UserName", "DisplayName", "EmailAddress", "LastModDateTime") =
                           (@UserName, @DisplayName, @EmailAddress, now())
                           WHERE "Uuid" = @Uuid
                            AND "IsActive";
                           """;

        var linesCount = await _dbConnection.ExecuteAsync(sql, user);
        if (linesCount < 1)
            throw new NotFoundException();

        return linesCount;
    }

    public async Task<int> DeactivateUserAsync(Guid userUuid)
    {
        const string sql = """
                           UPDATE "public"."Users"
                           SET "IsActive" = false, "LastModDateTime" = now()
                           WHERE "Uuid" = @userUuid
                            AND "IsActive";
                           """;

        var linesCount = await _dbConnection.ExecuteAsync(sql, new { userUuid });
        if (linesCount < 1)
            throw new NotFoundException();

        return linesCount;
    }
}
