using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record DelegationGrantedDomainEvent(
    Guid DelegationGrantExternalId,
    Guid DelegationDefinitionExternalId,
    Guid TenantExternalId) : DomainEvent;
