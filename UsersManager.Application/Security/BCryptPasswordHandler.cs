using UsersManager.Application.Interfaces;

namespace UsersManager.Application.Security;

public class BCryptPasswordHandler : IPasswordHandler
{
    public string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public bool ValidatePassword(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash);
}
