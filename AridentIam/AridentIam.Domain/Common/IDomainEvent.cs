using MediatR;

namespace AridentIam.Domain.Common;

public interface IDomainEvent : INotification
{
    DateTimeOffset OccurredAt { get; }
}
