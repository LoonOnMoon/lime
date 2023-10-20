using Lime.Domain.User.Entities;

namespace Lime.Domain.User.DomainEvents;

public class AdminInviteCreatedDomainEvent : UserDomainEvent
{
    public required AdminInvite AdminInvite { get; init; }
}