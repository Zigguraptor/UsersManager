using MediatR;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Friendship.Commands.FriendRequest;
using UsersManager.Application.Friendship.Queries;

namespace UsersManager.WebApi.Controllers;

public sealed class FriendshipController : BaseController
{
    private readonly ISender _sender;

    public FriendshipController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
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
    public async Task<IActionResult> SendFriendInviteAsync([FromBody] FriendInviteDto friendInviteDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var command = new FriendInviteCommand(friendInviteDto);
        try
        {
            if (await _sender.Send(command))
                return Ok();

            return BadRequest("Пользователи уже друзья");
        }
        catch (Exception e)
        {
            switch (e)
            {
                case PostgresException { SqlState: "23505" }:
                    return BadRequest("Запрос уже отправлен");
                case PostgresException { MessageText: "UserNotFound" }:
                    return BadRequest("Пользователь не найден");
                default:
                    throw;
            }
        }
    }
}
