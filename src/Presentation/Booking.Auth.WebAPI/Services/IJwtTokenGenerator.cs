using System.Security.Claims;

namespace Booking.Auth.WebAPI.Services;

public interface IJwtTokenGenerator
{
    string GenerateAccessToken(string email, string? roleName);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}