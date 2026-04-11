using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record PrincipalCreatedDomainEvent(Guid PrincipalExternalId, Guid TenantExternalId) : DomainEvent;