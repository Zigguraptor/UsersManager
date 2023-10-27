using MediatR;

namespace UsersManager.Application.Users.Commands.CreateUser;

public class CreateUserCommand : IRequest<Guid>
{
    public CreateUserCommand(CreateUserDto createUserDto) => CreateUserDto = createUserDto;

    public CreateUserDto CreateUserDto { get; set; }
}
