using MediatR;

namespace UsersManager.Application.Users.Commands.UpdateUser;

public class UpdateUserCommand : IRequest
{
    public UpdateUserCommand(UpdateUserDto updateUserDto)
    {
        UpdateUserDto = updateUserDto;
    }

    public UpdateUserDto UpdateUserDto { get; set; }
}
