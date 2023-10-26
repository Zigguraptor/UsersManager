using AutoMapper;
using MediatR;
using UsersManager.Application.Interfaces;
using UsersManager.Domain;

namespace UsersManager.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<User>(request.UpdateUserDto);
        
        return _usersRepository.UpdateUserAsync(user);
    }
}
