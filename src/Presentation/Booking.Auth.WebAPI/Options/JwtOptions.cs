namespace Booking.Auth.WebAPI.Options;

public class JwtOptions
{
    public static string Key = "Jwt";
    
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecretKey { get; set; }
}   