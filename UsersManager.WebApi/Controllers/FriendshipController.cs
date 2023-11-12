using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Friendship.Commands.CancelFriend;
using UsersManager.Application.Friendship.Commands.DeleteFriend;
using UsersManager.Application.Friendship.Commands.FriendRequest;
using UsersManager.Application.Friendship.Queries.FriendInvites;
using UsersManager.Application.Friendship.Queries.UserFriends;

namespace UsersManager.WebApi.Controllers;

public sealed class FriendshipController : BaseController
{
    private readonly ISender _sender;

    public FriendshipController(ISender sender) => _sender = sender;

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetFriendInvitesAsync()
    {
        var identityName = User.Identity?.Name;
        if (identityName == null || !Guid.TryParse(identityName, out var thisUserUuid))
            return Forbid();

        var friendInvitesQuery = new FriendInvitesQuery(thisUserUuid);
        var friendVms = await _sender.Send(friendInvitesQuery);

        return Ok(friendVms);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CancelFriendAsync([FromQuery] string user2Name)
    {
        var identityName = User.Identity?.Name;
        if (identityName == null || !Guid.TryParse(identityName, out var thisUserUuid))
            return Forbid();

        var cancelFriendCommand = new CancelFriendCommand(thisUserUuid, user2Name);
        await _sender.Send(cancelFriendCommand);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFriendAsync([FromQuery] string friendName)
    {
        var identityName = User.Identity?.Name;
        if (identityName == null || !Guid.TryParse(identityName, out var thisUserUuid))
            return Forbid();

        var deleteFriendCommand = new DeleteFriendCommand(thisUserUuid, friendName);
        await _sender.Send(deleteFriendCommand);

        return Ok();
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> UserFriendsAsync([FromQuery] string userName)
    {
        try
        {
            var friendVms = await _sender.Send(new UserFriendsQuery(userName));
            return Ok(friendVms);
        }
        catch (Exception e)
        {
            if (e is NotFoundException)
                return NotFound("Пользователь не найден");

            throw;
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SendFriendInviteAsync([FromQuery] string recipientName)
    {
        var identityName = User.Identity?.Name;
        if (identityName == null || !Guid.TryParse(identityName, out var thisUserUuid))
            return Forbid();

        var command = new FriendInviteCommand(thisUserUuid, recipientName);
        await _sender.Send(command);

        return Ok();
    }
}
