using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using UsersManager.Application.Interfaces;
using UsersManager.Domain;

namespace UsersManager.Application.Security;

public class JwtTokenGenerator : ITokenGenerator
{
    public string GenerateToken(User user, bool isAdmin = false)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Uuid.ToString()),
        };

        if (isAdmin)
            claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

        var jwt = new JwtSecurityToken(
            audience: "text-audience.com",
            issuer: "test-issuer.com",
            expires: DateTime.Now.AddHours(1),
            claims: claims,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey("eb58ed4d-46e3-46a5-ab25-55cb1412f1e6"u8.ToArray()),
                SecurityAlgorithms.HmacSha256));

        return "Bearer " + new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}
