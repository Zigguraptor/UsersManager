using MediatR;

namespace UsersManager.Application.Friendship.Queries.FriendInvites;

public class FriendInvitesQuery : IRequest<IEnumerable<FriendVm>>
{
    public readonly Guid UserUuid;

    public FriendInvitesQuery(Guid userUuid) => UserUuid = userUuid;
}
