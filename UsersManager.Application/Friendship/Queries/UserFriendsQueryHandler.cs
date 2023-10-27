using MediatR;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Friendship.Queries;

public class UserFriendsQueryHandler : IRequestHandler<UserFriendsQuery, IEnumerable<FriendVm>>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public UserFriendsQueryHandler(IFriendshipRepository friendshipRepository) =>
        _friendshipRepository = friendshipRepository;

    public Task<IEnumerable<FriendVm>> Handle(UserFriendsQuery request, CancellationToken cancellationToken) =>
        _friendshipRepository.GetFriendsAsync(request.UserName);
}
