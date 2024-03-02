namespace Booking.Auth.WebAPI.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(string email, string? roleName);
}