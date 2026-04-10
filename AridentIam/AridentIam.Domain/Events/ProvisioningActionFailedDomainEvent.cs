using AridentIam.Domain.Common;

namespace AridentIam.Domain.Events;

public sealed record ProvisioningActionFailedDomainEvent(
    Guid ProvisioningActionExternalId,
    Guid TenantExternalId,
    string ResultMessage) : DomainEvent;
