using MediatR;

namespace UsersManager.Application.Friendship.Commands.FriendRequest;

public class FriendInviteCommand : IRequest<bool>
{
    public FriendInviteCommand(FriendInviteDto friendInviteDto) => FriendInviteDto = friendInviteDto;

    public FriendInviteDto FriendInviteDto { get; set; }
}
