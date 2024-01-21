namespace Booking.Auth.Application.Features.AuthFeatures;

public sealed record AuthenticateResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}