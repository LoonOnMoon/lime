namespace Lime.Domain.Common;

public class ResultWithDomainEvents<TResult, TDomainEvent>
{
    public required TResult Result { get; set; }

    public List<TDomainEvent> DomainEvents { get; set; } = new();
}