using Lime.Application.Authentication.Common;

namespace Lime.Application.Authentication.Queries.Login;

public record LoginQuery(
    string UserNameOrEmail,
    string Password) : IRequest<AuthenticationResult>;