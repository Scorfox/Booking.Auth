using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Booking.Auth.Application.Infrastructure;
using Booking.Auth.WebAPI.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Booking.Auth.Application.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtOptions _options;
    
    public JwtTokenGenerator(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public string GenerateToken(string email, string? roleName)
    {
        var key = Encoding.ASCII.GetBytes(_options.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, roleName!),
            }),
            Expires = DateTime.UtcNow.AddMinutes(5),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}