namespace AridentIam.Domain.Common;

public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}
