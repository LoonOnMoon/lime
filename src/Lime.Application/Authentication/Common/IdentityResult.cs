namespace Lime.Application.Authentication.Common;

public record IdentityResult(
    string Id,
    string UserName,
    string Token);