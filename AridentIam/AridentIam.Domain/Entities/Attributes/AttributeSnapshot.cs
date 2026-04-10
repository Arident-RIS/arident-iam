using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Attributes;

public sealed class AttributeSnapshot : AuditableEntity
{
    private AttributeSnapshot() { }
    public Guid AttributeSnapshotExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string SnapshotType { get; private set; } = null!;
    public Guid? PrincipalExternalId { get; private set; }
    public Guid? ResourceInstanceReferenceExternalId { get; private set; }
    public DateTimeOffset CapturedAt { get; private set; }
    public string AttributesJson { get; private set; } = null!;
    public string? SourceVersion { get; private set; }

    public static AttributeSnapshot Create(Guid tenantExternalId, string snapshotType, DateTimeOffset capturedAt, string attributesJson, Guid? principalExternalId, Guid? resourceInstanceReferenceExternalId, string? sourceVersion, string createdBy)
    {
        if (!principalExternalId.HasValue && !resourceInstanceReferenceExternalId.HasValue)
            throw new DomainException("An attribute snapshot must target either a principal or a resource instance.");

        var entity = new AttributeSnapshot
        {
            AttributeSnapshotExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            SnapshotType = Guard.AgainstNullOrWhiteSpace(snapshotType, nameof(snapshotType)),
            CapturedAt = capturedAt,
            AttributesJson = Guard.AgainstNullOrWhiteSpace(attributesJson, nameof(attributesJson)),
            PrincipalExternalId = principalExternalId,
            ResourceInstanceReferenceExternalId = resourceInstanceReferenceExternalId,
            SourceVersion = string.IsNullOrWhiteSpace(sourceVersion) ? null : sourceVersion.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}