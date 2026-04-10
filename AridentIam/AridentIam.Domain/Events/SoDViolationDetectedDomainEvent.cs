using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Events;

public sealed record SoDViolationDetectedDomainEvent(
    Guid SoDViolationExternalId,
    Guid SoDRuleExternalId,
    Guid PrincipalExternalId,
    Guid TenantExternalId,
    SeverityLevel Severity) : DomainEvent;
