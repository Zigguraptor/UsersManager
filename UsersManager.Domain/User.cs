namespace UsersManager.Domain;

public class User : CommonCredentials
{
    public Guid Uuid { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsAdmin { get; set; } = false;
    public required string UserName { get; set; }
    public required string DisplayName { get; set; }
    public required string EmailAddress { get; set; }
    public required string PasswordHash { get; set; }

    public required DateOnly Dob { get; set; }
}
