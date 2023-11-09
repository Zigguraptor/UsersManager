using MediatR;

namespace UsersManager.Application.Friendship.Commands.DeleteFriend;

public class DeleteFriendCommand : IRequest
{
    public readonly Guid OwnerName;
    public readonly string FriendName;

    public DeleteFriendCommand(Guid ownerName, string friendName)
    {
        OwnerName = ownerName;
        FriendName = friendName;
    }
}
