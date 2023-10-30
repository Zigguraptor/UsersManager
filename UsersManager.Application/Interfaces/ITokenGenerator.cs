using UsersManager.Domain;

namespace UsersManager.Application.Interfaces;

public interface ITokenGenerator
{
    public string GenerateToken(User user, bool isAdmin = false);
}
