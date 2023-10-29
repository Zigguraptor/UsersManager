using MediatR;

namespace UsersManager.Application.Users.Queries.Login;

public class LoginQuery : IRequest<string?>
{
    public LoginQuery(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }

    public string UserName { get; set; }
    public string Password { get; set; }
}
