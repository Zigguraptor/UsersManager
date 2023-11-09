using MediatR;

namespace UsersManager.Application.Friendship.Commands.CancelFriend;

public class CancelFriendCommand : IRequest
{
    public readonly Guid User1Uuid;
    public readonly string User2Name;

    public CancelFriendCommand(Guid user1Uuid, string user2Name)
    {
        User1Uuid = user1Uuid;
        User2Name = user2Name;
    }
}
