using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record SessionRevokedDomainEvent(Guid SessionExternalId) : DomainEvent;
