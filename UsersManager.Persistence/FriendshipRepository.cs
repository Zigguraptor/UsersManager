using System.Data;
using Dapper;
using UsersManager.Application.Friendship.Commands.FriendRequest;
using UsersManager.Application.Interfaces;

namespace UsersManager.Persistence;

public class FriendshipRepository : IFriendshipRepository
{
    private readonly IDbConnection _dbConnection;

    public FriendshipRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
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
