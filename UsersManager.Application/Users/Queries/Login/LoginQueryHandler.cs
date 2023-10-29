using MediatR;
using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Users.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, string?>
{
    private readonly IPasswordHandler _passwordHandler;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUsersRepository _usersRepository;

    public LoginQueryHandler(IPasswordHandler passwordHandler, ITokenGenerator tokenGenerator,
        IUsersRepository usersRepository)
    {
        _passwordHandler = passwordHandler;
        _tokenGenerator = tokenGenerator;
        _usersRepository = usersRepository;
    }

    public async Task<string?> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _usersRepository.GetUserAsync(userName: request.UserName);
        if (user == null) throw new ArgumentException();
        if (_passwordHandler.ValidatePassword(request.Password, user.PasswordHash))
            return _tokenGenerator.GenerateToken(user);

        return null;
    }
}
