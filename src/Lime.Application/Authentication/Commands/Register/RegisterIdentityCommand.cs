using Lime.Application.Authentication.Common;

using MediatR;

namespace Lime.Application.Authentication.Commands.Register;

public record RegisterIdentityCommand(
    string UserName,
    string Email,
    string Password) : IRequest<IdentityResult>;