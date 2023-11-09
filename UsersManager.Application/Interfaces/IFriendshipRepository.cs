using UsersManager.Application.Friendship.Queries;
using UsersManager.Application.Friendship.Queries.FriendInvites;

namespace UsersManager.Application.Interfaces;

public interface IFriendshipRepository
{
    public Task<IEnumerable<FriendVm>> GetFriendsAsync(string userName);
    public Task FriendInviteAsync(Guid senderUuid, Guid recipientUuid);
    public Task<int> DeleteFriendAsync(Guid ownerUuid, string friendName);
    public Task<int> DeleteFriendInviteAsync(Guid user1Uuid, string user2Name);
    public Task<IEnumerable<FriendVm>> GetFriendInvites(FriendInvitesQuery request);
}
