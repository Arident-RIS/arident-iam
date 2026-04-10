using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Attributes;

public sealed class AttributeDefinition : AggregateRoot
{
    private AttributeDefinition() { }
    public Guid AttributeDefinitionExternalId { get; private set; }
    public Guid AttributeSchemaExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public bool IsSystemOwned => !TenantExternalId.HasValue;
    public string AttributeName { get; private set; } = null!;
    public AttributeDataType DataType { get; private set; }
    public bool IsMultiValued { get; private set; }
    public bool IsPolicyUsable { get; private set; }
    public bool IsSensitive { get; private set; }
    public string? DefaultValue { get; private set; }
    public string? ValidationRule { get; private set; }
    public AttributeSourceType SourceType { get; private set; }

    public static AttributeDefinition Create(Guid attributeSchemaExternalId, Guid? tenantExternalId, string attributeName, AttributeDataType dataType, bool isMultiValued, bool isPolicyUsable, bool isSensitive, AttributeSourceType sourceType, string? defaultValue, string? validationRule, string createdBy)
    {
        var entity = new AttributeDefinition
        {
            AttributeDefinitionExternalId = Guid.NewGuid(),
            AttributeSchemaExternalId = Guard.AgainstDefault(attributeSchemaExternalId, nameof(attributeSchemaExternalId)),
            TenantExternalId = tenantExternalId,
            AttributeName = Guard.AgainstNullOrWhiteSpace(attributeName, nameof(attributeName)),
            DataType = dataType,
            IsMultiValued = isMultiValued,
            IsPolicyUsable = isPolicyUsable,
            IsSensitive = isSensitive,
            SourceType = sourceType,
            DefaultValue = string.IsNullOrWhiteSpace(defaultValue) ? null : defaultValue.Trim(),
            ValidationRule = string.IsNullOrWhiteSpace(validationRule) ? null : validationRule.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void UpdateRules(string? validationRule, string? defaultValue, string updatedBy)
    {
        ValidationRule = string.IsNullOrWhiteSpace(validationRule) ? null : validationRule.Trim();
        DefaultValue = string.IsNullOrWhiteSpace(defaultValue) ? null : defaultValue.Trim();
        Touch(updatedBy);
    }
}