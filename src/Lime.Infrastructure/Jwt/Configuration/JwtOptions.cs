using System.ComponentModel.DataAnnotations;

namespace Lime.Infrastructure.Jwt.Configuration;

public sealed class JwtOptions
{
    [Required]
    public required string Issuer { get; init; }

    [Required]
    public required string Audience { get; init; }

    [Required]
    public required string SecretKey { get; init; }

    [Required]
    public required TimeSpan ExpiresIn { get; init; }
}