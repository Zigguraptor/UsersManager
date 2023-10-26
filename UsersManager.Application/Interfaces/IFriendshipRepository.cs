using UsersManager.Application.Friendship.Commands.FriendRequest;
using UsersManager.Application.Friendship.Queries;

namespace UsersManager.Application.Interfaces;

public interface IFriendshipRepository
{
    public Task<IEnumerable<FriendVm>> GetFriendsAsync(string userName);
    public Task<bool> FriendshipExistsAsync(Guid user1Uuid, Guid user2Uuid);
    public Task FriendInviteAsync(FriendInviteDto friendInviteDto);
}
