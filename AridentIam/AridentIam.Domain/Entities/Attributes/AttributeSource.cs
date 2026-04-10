using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Attributes;

public sealed class AttributeSource : AggregateRoot
{
    private AttributeSource() { }
    public Guid AttributeSourceExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public bool IsSystemOwned => !TenantExternalId.HasValue;
    public string SourceName { get; private set; } = null!;
    public AttributeSourceType SourceType { get; private set; }
    public string? ConnectorReference { get; private set; }
    public int Priority { get; private set; }
    public RefreshMode RefreshMode { get; private set; }
    public bool IsActive { get; private set; }

    public static AttributeSource Create(Guid? tenantExternalId, string sourceName, AttributeSourceType sourceType, int priority, RefreshMode refreshMode, string? connectorReference, string createdBy)
    {
        var entity = new AttributeSource
        {
            AttributeSourceExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            SourceName = Guard.AgainstNullOrWhiteSpace(sourceName, nameof(sourceName)),
            SourceType = sourceType,
            Priority = Guard.AgainstNegative(priority, nameof(priority)),
            RefreshMode = refreshMode,
            ConnectorReference = string.IsNullOrWhiteSpace(connectorReference) ? null : connectorReference.Trim(),
            IsActive = true
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Activate(string updatedBy) { IsActive = true; Touch(updatedBy); }
    public void Deactivate(string updatedBy) { IsActive = false; Touch(updatedBy); }
}