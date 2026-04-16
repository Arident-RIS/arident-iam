using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Auditing;

public sealed class AuditEvent : BaseEntity
{
    private AuditEvent() { }
    public Guid AuditEventExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string EventType { get; private set; } = null!;
    public string EventCategory { get; private set; } = null!;
    public Guid? ActorPrincipalExternalId { get; private set; }
    public string TargetType { get; private set; } = null!;
    public Guid? TargetId { get; private set; }
    public AuditOutcome Outcome { get; private set; }
    public string? ReasonCode { get; private set; }
    public string? IpAddress { get; private set; }
    public string? CorrelationId { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
    public string? PayloadJson { get; private set; }
    public string CreatedBy { get; private set; } = null!;

    public static AuditEvent Create(
        Guid tenantExternalId,
        string eventType,
        string eventCategory,
        string targetType,
        AuditOutcome outcome,
        Guid? actorPrincipalExternalId = null,
        Guid? targetId = null,
        string? reasonCode = null,
        string? ipAddress = null,
        string? correlationId = null,
        string? payloadJson = null,
        string? createdBy = null)
    {
        return new AuditEvent
        {
            AuditEventExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            EventType = Guard.AgainstNullOrWhiteSpace(eventType, nameof(eventType)),
            EventCategory = Guard.AgainstNullOrWhiteSpace(eventCategory, nameof(eventCategory)),
            ActorPrincipalExternalId = actorPrincipalExternalId,
            TargetType = Guard.AgainstNullOrWhiteSpace(targetType, nameof(targetType)),
            TargetId = targetId,
            Outcome = outcome,
            ReasonCode = string.IsNullOrWhiteSpace(reasonCode) ? null : reasonCode.Trim(),
            IpAddress = string.IsNullOrWhiteSpace(ipAddress) ? null : ipAddress.Trim(),
            CorrelationId = string.IsNullOrWhiteSpace(correlationId) ? null : correlationId.Trim(),
            OccurredAt = DateTimeOffset.UtcNow,
            PayloadJson = string.IsNullOrWhiteSpace(payloadJson) ? null : payloadJson.Trim(),
            CreatedBy = string.IsNullOrWhiteSpace(createdBy) ? "system" : createdBy.Trim()
        };
    }
}
