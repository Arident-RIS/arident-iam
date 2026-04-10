using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Attributes;

public sealed class AttributeSourceMapping : AuditableEntity
{
    private AttributeSourceMapping() { }
    public Guid AttributeSourceMappingExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid AttributeDefinitionExternalId { get; private set; }
    public Guid AttributeSourceExternalId { get; private set; }
    public string ExternalFieldName { get; private set; } = null!;
    public string? TransformationRule { get; private set; }
    public string? FallbackValue { get; private set; }

    public static AttributeSourceMapping Create(Guid tenantExternalId, Guid attributeDefinitionExternalId, Guid attributeSourceExternalId, string externalFieldName, string? transformationRule, string? fallbackValue, string createdBy)
    {
        var entity = new AttributeSourceMapping
        {
            AttributeSourceMappingExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            AttributeDefinitionExternalId = Guard.AgainstDefault(attributeDefinitionExternalId, nameof(attributeDefinitionExternalId)),
            AttributeSourceExternalId = Guard.AgainstDefault(attributeSourceExternalId, nameof(attributeSourceExternalId)),
            ExternalFieldName = Guard.AgainstNullOrWhiteSpace(externalFieldName, nameof(externalFieldName)),
            TransformationRule = string.IsNullOrWhiteSpace(transformationRule) ? null : transformationRule.Trim(),
            FallbackValue = string.IsNullOrWhiteSpace(fallbackValue) ? null : fallbackValue.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}