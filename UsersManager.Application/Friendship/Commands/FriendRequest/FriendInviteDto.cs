namespace UsersManager.Application.Friendship.Commands.FriendRequest;

public class FriendInviteDto
{
    public FriendInviteDto(Guid fromUserUuid, Guid toUserUuid)
    {
        FromUserUuid = fromUserUuid;
        ToUserUuid = toUserUuid;
    }

    public Guid FromUserUuid { get; set; }
    public Guid ToUserUuid { get; set; }
}
