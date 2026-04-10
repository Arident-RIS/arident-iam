using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Organizations;

public sealed class OrgUnitType : AggregateRoot
{
    private OrgUnitType() { }
    public Guid OrgUnitTypeExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid OrgSchemaExternalId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public int HierarchyLevel { get; private set; }

    public static OrgUnitType Create(Guid tenantExternalId, Guid orgSchemaExternalId, string code, string name, int hierarchyLevel, string createdBy)
    {
        if (hierarchyLevel < 1) throw new DomainException("HierarchyLevel must be greater than zero.");
        var entity = new OrgUnitType
        {
            OrgUnitTypeExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            OrgSchemaExternalId = Guard.AgainstDefault(orgSchemaExternalId, nameof(orgSchemaExternalId)),
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            HierarchyLevel = hierarchyLevel
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
