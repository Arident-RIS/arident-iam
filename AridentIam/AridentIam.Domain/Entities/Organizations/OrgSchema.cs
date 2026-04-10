using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Organizations;

public sealed class OrgSchema : AggregateRoot
{
    private OrgSchema() { }
    public Guid OrgSchemaExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsDefault { get; private set; }

    public static OrgSchema Create(Guid tenantExternalId, string name, string? description, bool isDefault, string createdBy)
    {
        var entity = new OrgSchema
        {
            OrgSchemaExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            IsDefault = isDefault
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
