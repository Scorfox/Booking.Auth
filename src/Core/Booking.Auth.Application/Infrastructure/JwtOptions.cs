namespace Booking.Auth.Application.Infrastructure;

public class JwtOptions
{
    public static string Key = "Jwt";
    
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
    public TimeSpan LifeTime { get; set; }
}   