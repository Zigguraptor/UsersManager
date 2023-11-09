using MediatR;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Friendship.Queries.FriendInvites;

public class FriendInvitesQueryHandler : IRequestHandler<FriendInvitesQuery, IEnumerable<FriendVm>>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public FriendInvitesQueryHandler(IFriendshipRepository friendshipRepository) =>
        _friendshipRepository = friendshipRepository;

    public Task<IEnumerable<FriendVm>> Handle(FriendInvitesQuery request, CancellationToken cancellationToken) =>
        _friendshipRepository.GetFriendInvites(request);
}
