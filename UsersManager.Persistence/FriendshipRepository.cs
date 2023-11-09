using System.Data;
using Dapper;
using Microsoft.AspNetCore.Http;
using Npgsql;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Friendship.Queries;
using UsersManager.Application.Friendship.Queries.FriendInvites;
using UsersManager.Application.Friendship.Queries.UserFriends;
using UsersManager.Application.Interfaces;

namespace UsersManager.Persistence;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly IDbConnection _dbConnection;

    public FriendshipRepository(IDbConnection dbConnection) =>
        _dbConnection = dbConnection;

    public Task<IEnumerable<FriendVm>> GetFriendsAsync(string userName)
    {
        userName = userName.ToLower();

        const string sql = """
                           WITH "friends_uuids"
                                    AS (
                                       SELECT "User2Uuid" AS "uuid"
                                       FROM "UsersFriends" "uf"
                                           JOIN "Users" "u"
                                               ON "User1Uuid" = "u"."Uuid"
                                       WHERE lower("u"."UserName") = @userName
                                       UNION ALL
                                       SELECT "User1Uuid" AS "uuid"
                                       FROM "UsersFriends" "uf"
                                           JOIN "Users" "u"
                                               ON "User2Uuid" = "u"."Uuid"
                                       WHERE lower("u"."UserName") = @userName
                                       )
                           SELECT "u".*
                           FROM "friends_uuids" "fu"
                               JOIN "Users" "u"
                                   ON "u"."Uuid" = "fu"."uuid"
                                   AND "u"."IsActive";
                           """;
        return _dbConnection.QueryAsync<FriendVm>(sql, new { userName });
    }

    public Task<bool> FriendshipExistsAsync(Guid user1Uuid, Guid user2Uuid)
    {
        const string query = "SELECT \"public\".\"is_friends_func\"(@user1Uuid, @user2Uuid);";
        return _dbConnection.QueryFirstAsync<bool>(query, new { user1Uuid, user2Uuid });
    }

    public async Task FriendInviteAsync(Guid senderUuid, Guid recipientUuid)
    {
        const string sql = "CALL friend_invite_proc(@senderUuid, @recipientUuid);";

        if (await FriendshipExistsAsync(senderUuid, recipientUuid))
            throw new HttpException(StatusCodes.Status400BadRequest, "Пользователи уже друзья.");

        try
        {
            await _dbConnection.ExecuteAsync(sql, new { senderUuid, recipientUuid });
        }
        catch (Exception e)
        {
            switch (e)
            {
                case PostgresException { SqlState: "23505" }:
                    throw new HttpException(StatusCodes.Status400BadRequest, "Запрос уже отправлен.");
                case PostgresException { MessageText: "UserNotFound" }:
                    throw new HttpException(StatusCodes.Status400BadRequest, "Пользователь не найден.");
                default:
                    throw;
            }
        }
    }

    public Task<int> DeleteFriendAsync(Guid ownerUuid, string friendName)
    {
        const string sql = """
                           DELETE
                           FROM "public"."UsersFriends"
                           WHERE ("User1Uuid" = @ownerUuid
                               AND "User2Uuid" = (
                                                 SELECT "u"."Uuid"
                                                 FROM "Users" "u"
                                                 WHERE "u"."UserName" = @friendName
                                                 ))
                              OR ("User1Uuid" = (
                                               SELECT "u"."Uuid"
                                               FROM "Users" "u"
                                               WHERE "u"."UserName" = @friendName
                                               )
                               AND "User2Uuid" = @ownerUuid);

                           """;

        return _dbConnection.ExecuteAsync(sql, new { ownerUuid, friendName });
    }

    public Task<int> DeleteFriendInviteAsync(Guid user1Uuid, string user2Name)
    {
        const string sql = """
                           DELETE
                           FROM "FriendRequests"
                           WHERE ("User1Uuid" = @user1Uuid
                               AND "User2Uuid" = (
                                                 SELECT "Users"."Uuid"
                                                 FROM "public"."Users"
                                                 WHERE "UserName" = @user2Name
                                                 ))
                              OR ("User1Uuid" = (
                                                SELECT "Users"."Uuid"
                                                FROM "public"."Users"
                                                WHERE "UserName" = @user2Name
                                                )
                               AND "User2Uuid" = @user1Uuid);
                           """;
        return _dbConnection.ExecuteAsync(sql, new { user1Uuid, user2Name });
    }

    public Task<IEnumerable<FriendVm>> GetFriendInvites(FriendInvitesQuery request)
    {
        const string sql = """
                           SELECT "u".*
                           FROM "Users" "u"
                               JOIN "FriendRequests" "fr"
                                   ON "fr"."User2Uuid" = @UserUuid
                                   AND "u"."Uuid" = "fr"."User1Uuid";
                           """;

        return _dbConnection.QueryAsync<FriendVm>(sql, new { request.UserUuid });
    }
}
