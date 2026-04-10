using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Governance;

public sealed class ComplianceRecord : AuditableEntity
{
    private ComplianceRecord() { }
    public Guid ComplianceRecordExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public ComplianceRecordType RecordType { get; private set; }
    public string ReferenceType { get; private set; } = null!;
    public string ReferenceId { get; private set; } = null!;
    public string EvidenceLocation { get; private set; } = null!;
    public DateTimeOffset CapturedAt { get; private set; }
    public DateTimeOffset RetentionUntil { get; private set; }

    public static ComplianceRecord Create(Guid tenantExternalId, ComplianceRecordType recordType, string referenceType, string referenceId, string evidenceLocation, DateTimeOffset capturedAt, DateTimeOffset retentionUntil, string createdBy)
    {
        if (retentionUntil < capturedAt)
            throw new DomainException("Retention date cannot be earlier than captured date.");

        var entity = new ComplianceRecord
        {
            ComplianceRecordExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            RecordType = recordType,
            ReferenceType = Guard.AgainstNullOrWhiteSpace(referenceType, nameof(referenceType)),
            ReferenceId = Guard.AgainstNullOrWhiteSpace(referenceId, nameof(referenceId)),
            EvidenceLocation = Guard.AgainstNullOrWhiteSpace(evidenceLocation, nameof(evidenceLocation)),
            CapturedAt = capturedAt,
            RetentionUntil = retentionUntil
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}