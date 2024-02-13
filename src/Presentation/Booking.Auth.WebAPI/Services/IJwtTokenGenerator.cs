using Booking.Auth.Application.Features.AuthFeatures;

namespace Booking.Auth.WebAPI.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(string email, string? roleName);

    string GenerateRefreshToken();

    Task<AuthenticateResponse> RefreshToken(string token, string refreshToken);
}