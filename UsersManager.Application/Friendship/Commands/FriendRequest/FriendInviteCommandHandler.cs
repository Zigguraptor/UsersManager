using MediatR;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Friendship.Commands.FriendRequest;

public class FriendInviteCommandHandler : IRequestHandler<FriendInviteCommand, bool>
{
    private readonly IFriendshipRepository _friendshipRepository;

    public FriendInviteCommandHandler(IFriendshipRepository friendshipRepository) =>
        _friendshipRepository = friendshipRepository;

    public async Task<bool> Handle(FriendInviteCommand command, CancellationToken cancellationToken)
    {
        if (await _friendshipRepository.FriendshipExistsAsync(command.FriendInviteDto.FromUserUuid,
                command.FriendInviteDto.ToUserUuid))
            return false;

        await _friendshipRepository.FriendInviteAsync(command.FriendInviteDto);
        return true;
    }
}
