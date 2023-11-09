using MediatR;

namespace UsersManager.Application.Friendship.Commands.FriendRequest;

public class FriendInviteCommand : IRequest
{
    public readonly Guid SenderUuid;
    public readonly string ToUser2Name;

    public FriendInviteCommand(Guid senderUuid, string toUser2Name)
    {
        SenderUuid = senderUuid;
        ToUser2Name = toUser2Name;
    }
}
