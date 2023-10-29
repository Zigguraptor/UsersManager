namespace UsersManager.Application.Interfaces;

public interface IPasswordHandler
{
    public string HashPassword(string password);
    public bool ValidatePassword(string password, string hash);
}
