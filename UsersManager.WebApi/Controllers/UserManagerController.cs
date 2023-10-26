using MediatR;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Users.Commands.CreateUser;
using UsersManager.Application.Users.Commands.DeleteUser;
using UsersManager.Application.Users.Commands.UpdateUser;
using UsersManager.Application.Users.Queries;

namespace UsersManager.WebApi.Controllers;

public sealed class UserManagerController : BaseController
{
    private readonly ISender _sender;

    public UserManagerController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetUserInfoAsync(
        [FromQuery] Guid? guid, [FromQuery] string? userName, [FromQuery] string? emailAddress)
    {
        if (guid == null && userName == null && emailAddress == null)
            return BadRequest("Нужно указать что-то из перечисленного: guid, userName, emailAddress");

        var query = new UserInfoQuery
        {
            Guid = guid,
            UserName = userName,
            EmailAddress = emailAddress
        };

        var userInfoVm = await _sender.Send(query);
        return userInfoVm == null ? NotFound("Пользователь не найден") : Ok(userInfoVm);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var command = new CreateUserCommand(createUserDto);
        try
        {
            var guid = await _sender.Send(command);
            return Created($"/{guid}", guid);
        }
        catch (Exception e)
        {
            if (e is not PostgresException { SqlState: "23505" } pgEx) throw;
            switch (pgEx.ConstraintName)
            {
                case "Users_UserName_key":
                    return BadRequest("Имя пользователя уже занято");
                case "Users_EmailAddress_key":
                    return BadRequest("Адрес электронной почты уже занят");
                default:
                    throw;
            }
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);
        var command = new UpdateUserCommand(updateUserDto);

        try
        {
            await _sender.Send(command);
            return Ok();
        }
        catch (Exception e)
        {
            if (e is NotFoundException)
                return NotFound("Пользователь не найден");
            if (e is not PostgresException { SqlState: "23505" } pgEx) throw;
            switch (pgEx.ConstraintName)
            {
                case "Users_UserName_key":
                    return BadRequest("Имя пользователя уже занято");
                case "Users_EmailAddress_key":
                    return BadRequest("Адрес электронной почты уже занят");
                default:
                    throw;
            }
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUserAsync([FromBody] DeleteUserDto deleteUserDto)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

        var command = new DeleteUserCommand(deleteUserDto);

        try
        {
            await _sender.Send(command);
            return Ok();
        }
        catch (Exception e)
        {
            if (e is NotFoundException)
                return NotFound("Пользователь не найден");

            throw;
        }
    }
}
