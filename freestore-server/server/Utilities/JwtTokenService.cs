

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using server.Models.Authentication;

namespace server.Utilities;

public interface IJwtTokenService
{
    public string CreateAccessToken(int userId, int level);
}

public class JwtTokenService : IJwtTokenService
{
    private SymmetricSecurityKey _authSigningKey;
    private string _issuer;
    private string _audience;

    public JwtTokenService(IConfiguration configuration)
    {
        _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
        _issuer = configuration["JWT:ValidIssuer"];
        _audience = configuration["JWT:ValidAudience"];
    }

    public string CreateAccessToken(int userId, int level)
    {
        var authClaims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, Role.FromId(level))
        };

        var accessSecToken = new JwtSecurityToken
        (
            issuer: _issuer,
            audience: _audience,
            expires: DateTime.UtcNow.AddDays(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(_authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(accessSecToken);
    }
}