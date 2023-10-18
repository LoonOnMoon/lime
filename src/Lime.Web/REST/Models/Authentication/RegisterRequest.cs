namespace Lime.Web.REST.Models.Authentication;

public record RegisterRequest
{
    public string? UserName { get; init; }

    public string? Email { get; init; }

    public string? Password { get; init; }

    public string? Organization { get; init; }
}