using System.Data;
using Dapper;
using UsersManager.Application.Friendship.Commands.FriendRequest;
using UsersManager.Application.Friendship.Queries;
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

    public Task FriendInviteAsync(FriendInviteDto friendInviteDto)
    {
        const string sql = "CALL friend_invite_proc(@FromUserUuid, @ToUserUuid);";
        return _dbConnection.ExecuteAsync(sql, friendInviteDto);
    }
}
