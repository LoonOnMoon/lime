namespace Lime.Contracts.Authentication;

public record RegisterRequest(
    string Email,
    string Password);