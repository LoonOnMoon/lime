using System.ComponentModel.DataAnnotations;

namespace Lime.Infrastructure.Identity.Configuration;

public sealed class JwtOptions
{
    [Required]
    public required string Issuer { get; init; }

    [Required]
    public required string Audience { get; init; }

    [Required]
    public required string SecretKey { get; init; }
}