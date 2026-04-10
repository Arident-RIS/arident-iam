using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record UserCreatedDomainEvent(Guid UserExternalId, Guid TenantExternalId) : DomainEvent;
