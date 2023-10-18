namespace Lime.Application.Authentication.Common;

public record AuthenticationResult(
    string Token,
    string Organization,
    string UserName);