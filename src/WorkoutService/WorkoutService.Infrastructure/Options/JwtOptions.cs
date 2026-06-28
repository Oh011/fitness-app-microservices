namespace WorkoutService.Infrastructure.Options;

public class JwtOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audiance { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int ExpirationInHours { get; set; }
}
