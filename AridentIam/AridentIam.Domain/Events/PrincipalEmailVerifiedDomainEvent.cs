using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record PrincipalEmailVerifiedDomainEvent(Guid PrincipalExternalId, Guid TenantExternalId) : DomainEvent;