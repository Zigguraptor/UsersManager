using AutoMapper;
using MediatR;
using UsersManager.Application.Interfaces;
using UsersManager.Domain;

namespace UsersManager.Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordHandler _passwordHandler;

    public CreateUserCommandHandler(IMapper mapper, IUsersRepository usersRepository, IPasswordHandler passwordHandler)
    {
        _mapper = mapper;
        _usersRepository = usersRepository;
        _passwordHandler = passwordHandler;
    }

    public Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request.CreateUserDto);
        user.PasswordHash = _passwordHandler.HashPassword(request.CreateUserDto.Password);
        
        return _usersRepository.InsertUserAsync(user);
    }
}
