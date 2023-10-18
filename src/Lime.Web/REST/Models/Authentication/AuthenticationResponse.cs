namespace Lime.Web.REST.Models.Authentication;

public class AuthenticationResponse
{
    public required string Token { get; init; }

    public string? Organization { get; init; }

    public string? UserName { get; init; }
}