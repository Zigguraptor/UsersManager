using MediatR;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUsersRepository _usersRepository;

    public DeleteUserCommandHandler(IUsersRepository usersRepository) => _usersRepository = usersRepository;

    public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken) =>
        _usersRepository.DeactivateUserAsync(request.DeleteUserDto.UserUuid);
}
