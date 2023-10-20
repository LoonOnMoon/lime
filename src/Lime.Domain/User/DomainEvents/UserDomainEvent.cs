using Lime.Domain.Common;

namespace Lime.Domain.User.DomainEvents;

public abstract class UserDomainEvent : DomainEvent
{
    public required UserId RaisedBy { get; init; }
}