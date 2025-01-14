using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Infrastructure.JWT;

public class JwtHelper
{
    private readonly JwtSettings _jwtSettings;

    // IOptions<JwtSettings> kullanarak, JwtSettings'i DI ile alıyoruz
    public JwtHelper(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;
    }

    public string GenerateToken(User user)
    {
        var role = string.IsNullOrEmpty(user.Role) ? "User" : user.Role;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.IdentityNumber),
            new Claim(ClaimTypes.Name, user.IdentityNumber),
            new Claim(ClaimTypes.Role, role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
            return principal;
        }
        catch (SecurityTokenExpiredException)
        {
            throw new UnauthorizedAccessException("Token expired.");
        }
        catch (SecurityTokenException)
        {
            throw new UnauthorizedAccessException("Invalid token.");
        }
        catch (Exception)
        {
            throw new UnauthorizedAccessException("Token validation failed.");
        }
    }
}
