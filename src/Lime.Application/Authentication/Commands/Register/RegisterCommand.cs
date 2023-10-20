using Lime.Application.Authentication.Common;

namespace Lime.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password,
    string Organization) : IRequest<AuthenticationResult>;