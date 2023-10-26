using AutoMapper;
using MediatR;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Users.Queries;

public class UserInfoQueryHandler : IRequestHandler<UserInfoQuery, UserInfoVm?>
{
    private readonly IUsersRepository _usersRepository;
    private readonly IMapper _mapper;

    public UserInfoQueryHandler(IUsersRepository usersRepository, IMapper mapper)
    {
        _usersRepository = usersRepository;
        _mapper = mapper;
    }

    public async Task<UserInfoVm?> Handle(UserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserAsync(request.Guid, request.UserName, request.EmailAddress);
        return user == null ? null : _mapper.Map<UserInfoVm>(user);
    }
}
