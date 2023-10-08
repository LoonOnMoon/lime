using Lime.Application.Authentication.Common;

using MediatR;

namespace Lime.Application.Authentication.Queries.Login;

public record LoginIdentityQuery(
    string UserNameOrEmail,
    string Password) : IRequest<IdentityResult>;