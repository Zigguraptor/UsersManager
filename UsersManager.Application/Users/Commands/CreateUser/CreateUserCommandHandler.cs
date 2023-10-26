using AutoMapper;
using MediatR;
using UsersManager.Application.Interfaces;
using UsersManager.Domain;

namespace UsersManager.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request.CreateUserDto);

        return _usersRepository.InsertUserAsync(user);
    }
}
