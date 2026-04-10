using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Integrations;

public sealed class SyncMapping : AuditableEntity
{
    private SyncMapping() { }
    public Guid SyncMappingExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public bool IsSystemOwned => !TenantExternalId.HasValue;
    public Guid ExternalConnectorExternalId { get; private set; }
    public string SourceObjectType { get; private set; } = null!;
    public string SourceField { get; private set; } = null!;
    public string TargetEntity { get; private set; } = null!;
    public string TargetField { get; private set; } = null!;
    public string? TransformRule { get; private set; }

    public static SyncMapping Create(Guid? tenantExternalId, Guid externalConnectorExternalId, string sourceObjectType, string sourceField, string targetEntity, string targetField, string? transformRule, string createdBy)
    {
        var entity = new SyncMapping
        {
            SyncMappingExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            ExternalConnectorExternalId = Guard.AgainstDefault(externalConnectorExternalId, nameof(externalConnectorExternalId)),
            SourceObjectType = Guard.AgainstNullOrWhiteSpace(sourceObjectType, nameof(sourceObjectType)),
            SourceField = Guard.AgainstNullOrWhiteSpace(sourceField, nameof(sourceField)),
            TargetEntity = Guard.AgainstNullOrWhiteSpace(targetEntity, nameof(targetEntity)),
            TargetField = Guard.AgainstNullOrWhiteSpace(targetField, nameof(targetField)),
            TransformRule = string.IsNullOrWhiteSpace(transformRule) ? null : transformRule.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}