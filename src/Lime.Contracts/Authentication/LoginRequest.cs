namespace Lime.Contracts.Authentication;

public class LoginRequest
{
    public string? UserNameOrEmail { get; init; }

    public string? Password { get; init; }
}