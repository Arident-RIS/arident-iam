using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record PolicyPublishedDomainEvent(Guid PolicyVersionExternalId) : DomainEvent;
