using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record DelegationRevokedDomainEvent(
    Guid DelegationGrantExternalId,
    Guid TenantExternalId) : DomainEvent;
