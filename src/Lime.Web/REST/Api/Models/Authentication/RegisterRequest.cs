namespace Lime.Web.REST.Api.Models.Authentication;

public record RegisterRequest
{
    public string? UserName { get; init; }

    public string? Email { get; init; }

    public string? Password { get; init; }

    public string? Organization { get; init; }
}