using Lime.Application.Authentication.Common;

using MediatR;

namespace Lime.Application.Authentication.Queries.Login;

public record LoginQuery(
    string Email,
    string Password) : IRequest<AuthenticationResult>;