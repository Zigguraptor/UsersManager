namespace UsersManager.Application.Security;

public class JwtConfiguration
{
    public string Audience { get; set; } = null!;
    public string Issuer { get; set; } = null!;
}
