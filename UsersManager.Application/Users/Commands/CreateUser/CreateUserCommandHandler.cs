using AutoMapper;
using MediatR;
using UsersManager.Application.Interfaces;
using UsersManager.Domain;

namespace UsersManager.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IUsersRepository _usersRepository;

    public CreateUserCommandHandler(IMapper mapper, IUsersRepository usersRepository)
    {
        _mapper = mapper;
        _usersRepository = usersRepository;
    }

    public Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request.CreateUserDto);

        return _usersRepository.InsertUserAsync(user);
    }
}
