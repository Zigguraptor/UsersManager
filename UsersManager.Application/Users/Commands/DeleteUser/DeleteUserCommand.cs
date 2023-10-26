using MediatR;

namespace UsersManager.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
    public DeleteUserCommand(DeleteUserDto deleteUserDto)
    {
        DeleteUserDto = deleteUserDto;
    }

    public DeleteUserDto DeleteUserDto { get; set; }
}
