using Lime.Application.Users.Common;

namespace Lime.Application.Users.Commands.CreateAdminInvite;

public record CreateAdminInviteCommand(
    string Email,
    Guid CreatedBy) : IRequest<InviteResult>;