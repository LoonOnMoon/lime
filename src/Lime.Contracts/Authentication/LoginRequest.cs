namespace Lime.Contracts.Authentication;

public record LoginRequest(
    string UserNameOrEmail,
    string Password);