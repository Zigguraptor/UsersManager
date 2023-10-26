using UsersManager.Application.Friendship.Commands.FriendRequest;

namespace UsersManager.Application.Interfaces;

public interface IFriendshipRepository
{
    public Task<bool> FriendshipExistsAsync(Guid user1Uuid, Guid user2Uuid);
    public Task FriendInviteAsync(FriendInviteDto friendInviteDto);
}
