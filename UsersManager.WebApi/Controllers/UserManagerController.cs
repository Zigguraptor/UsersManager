using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using UsersManager.Application.Common.Exceptions;
using UsersManager.Application.Users.Commands.CreateUser;
using UsersManager.Application.Users.Commands.DeleteUser;
using UsersManager.Application.Users.Commands.UpdateUser;
using UsersManager.Application.Users.Queries.Login;
using UsersManager.Application.Users.Queries.UserInfo;

namespace UsersManager.WebApi.Controllers;

public sealed class UserManagerController : BaseController
{
    private readonly ILogger<UserManagerController> _logger;
    private readonly ISender _sender;

    public UserManagerController(ILogger<UserManagerController> logger, ISender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> LoginAsync(LoginQuery loginQuery)
    {
        var token = await _sender.Send(loginQuery);
        if (token == null)
        {
            _logger.LogInformation("Не удачная попытка входа c ip:{ipAddress}; login:{login};",
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                loginQuery.UserName);
            return BadRequest("Не верные данные для входа.");
        }

        _logger.LogInformation("Успешно выполнен вход с ip:{ipAddress}; login:{login};",
            HttpContext.Connection.RemoteIpAddress?.ToString(),
            loginQuery.UserName);
        
        return Ok(token);
    }

    [HttpGet]
    [Authorize]
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
    [AllowAnonymous]
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
    [Authorize]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto updateUserDto)
    {
        if (HttpContext.User.Identity?.Name == null || HttpContext.User.Identity.Name != updateUserDto.Uuid.ToString())
            return Forbid();

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
    [Authorize(Policy = "RequireAdministratorRole")]
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
