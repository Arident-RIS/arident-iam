using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Attributes;

public sealed class AttributeSchema : AggregateRoot
{
    private AttributeSchema() { }
    public Guid AttributeSchemaExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public bool IsSystemOwned => !TenantExternalId.HasValue;
    public string Name { get; private set; } = null!;
    public AttributeCategory Category { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    public static AttributeSchema Create(Guid? tenantExternalId, string name, AttributeCategory category, string? description, string createdBy)
    {
        var entity = new AttributeSchema
        {
            AttributeSchemaExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            Category = category,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            IsActive = true
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Activate(string updatedBy) { IsActive = true; Touch(updatedBy); }
    public void Deactivate(string updatedBy) { IsActive = false; Touch(updatedBy); }
}