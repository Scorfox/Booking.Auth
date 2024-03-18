namespace Booking.Auth.Application.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(string email, string? roleName, Guid? companyId = null);
}