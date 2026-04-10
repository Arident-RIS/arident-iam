using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Governance;

public sealed class SoDViolation : AggregateRoot
{
    private SoDViolation() { }
    public Guid SoDViolationExternalId { get; private set; }
    public Guid SoDRuleExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public SeverityLevel Severity { get; private set; }
    public DateTimeOffset DetectedAt { get; private set; }
    public ViolationState ViolationState { get; private set; }
    public string? ResolutionComment { get; private set; }

    public static SoDViolation Create(Guid tenantExternalId, Guid soDRuleExternalId, Guid principalExternalId, SeverityLevel severity, DateTimeOffset detectedAt, string createdBy)
    {
        var entity = new SoDViolation
        {
            SoDViolationExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            SoDRuleExternalId = Guard.AgainstDefault(soDRuleExternalId, nameof(soDRuleExternalId)),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            Severity = severity,
            DetectedAt = detectedAt,
            ViolationState = ViolationState.Detected
        };
        entity.SetCreationAudit(createdBy);
        entity.RaiseDomainEvent(new SoDViolationDetectedDomainEvent(entity.SoDViolationExternalId, entity.SoDRuleExternalId, entity.PrincipalExternalId, entity.TenantExternalId, entity.Severity));
        return entity;
    }

    public void Acknowledge(string updatedBy)
    {
        if (ViolationState != ViolationState.Detected)
            throw new DomainException("Only detected violations can be acknowledged.");
        ViolationState = ViolationState.Acknowledged;
        Touch(updatedBy);
    }

    public void Mitigate(string updatedBy, string? resolutionComment = null)
    {
        if (ViolationState is not (ViolationState.Detected or ViolationState.Acknowledged))
            throw new DomainException("Violation cannot be mitigated in its current state.");
        ViolationState = ViolationState.Mitigated;
        ResolutionComment = string.IsNullOrWhiteSpace(resolutionComment) ? null : resolutionComment.Trim();
        Touch(updatedBy);
    }

    public void Resolve(string updatedBy, string? resolutionComment = null)
    {
        if (ViolationState == ViolationState.Resolved)
            throw new DomainException("Violation is already resolved.");
        ViolationState = ViolationState.Resolved;
        ResolutionComment = string.IsNullOrWhiteSpace(resolutionComment) ? null : resolutionComment.Trim();
        Touch(updatedBy);
    }

    public void AcceptRisk(string updatedBy, string? resolutionComment = null)
    {
        if (ViolationState == ViolationState.Resolved)
            throw new DomainException("Cannot accept risk on a resolved violation.");
        ViolationState = ViolationState.Accepted;
        ResolutionComment = string.IsNullOrWhiteSpace(resolutionComment) ? null : resolutionComment.Trim();
        Touch(updatedBy);
    }
}