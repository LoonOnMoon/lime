using MediatR;

namespace Lime.Domain.Common;

public abstract class DomainEvent : INotification
{
    public DateTime RaisedOn { get; init; } = DateTime.UtcNow;

    public Guid Id { get; init; } = Guid.NewGuid();
}