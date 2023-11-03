using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UsersManager.Application.Interfaces;
using UsersManager.Domain;

namespace UsersManager.Application.Security;

public class JwtTokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;
    public JwtTokenGenerator(IConfiguration configuration) => _configuration = configuration;

    public string GenerateToken(User user, bool isAdmin = false)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Uuid.ToString()),
            new(ClaimTypes.DateOfBirth, user.Dob.ToString())
        };

        if (isAdmin)
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

        var expireDate = DateTime.UtcNow.AddSeconds(_configuration.GetValue<double>("DefaultJwtTokenLiveTimeS"));
        var jwtConfiguration = new JwtConfiguration();
        _configuration.Bind("DefaultJwtSecurityToken", jwtConfiguration);

        var jwt = new JwtSecurityToken(
            audience: jwtConfiguration.Audience,
            issuer: jwtConfiguration.Issuer,
            expires: expireDate,
            claims: claims,
            signingCredentials: CreateSigningCredentials());

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    private SigningCredentials CreateSigningCredentials()
    {
        var secretKey = Encoding.UTF8.GetBytes(
            _configuration.GetValue<string>("TokenValidationSecurityKey")
            ?? throw new InvalidOperationException());

        return new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);
    }
}
