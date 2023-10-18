using Lime.Application.Authentication.Common;

using MediatR;

namespace Lime.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password,
    string Organization) : IRequest<AuthenticationResult>;