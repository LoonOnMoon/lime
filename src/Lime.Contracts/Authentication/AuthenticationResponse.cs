namespace Lime.Contracts.Authentication;

public record AuthenticationResponse(
    string Token,
    string Organization,
    string UserName);