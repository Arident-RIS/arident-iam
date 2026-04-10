using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Organizations;

public sealed class OrgUnit : AggregateRoot
{
    private OrgUnit() { }
    public Guid OrganizationUnitExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid OrgSchemaExternalId { get; private set; }
    public Guid OrgUnitTypeExternalId { get; private set; }
    public Guid? ParentOrganizationUnitExternalId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Path { get; private set; } = null!;
    public int Depth { get; private set; }
    public bool IsActive { get; private set; }

    public static OrgUnit Create(Guid tenantExternalId, Guid orgSchemaExternalId, Guid orgUnitTypeExternalId, Guid? parentOrganizationUnitExternalId, string code, string name, string path, int depth, string createdBy)
    {
        var entity = new OrgUnit
        {
            OrganizationUnitExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            OrgSchemaExternalId = Guard.AgainstDefault(orgSchemaExternalId, nameof(orgSchemaExternalId)),
            OrgUnitTypeExternalId = Guard.AgainstDefault(orgUnitTypeExternalId, nameof(orgUnitTypeExternalId)),
            ParentOrganizationUnitExternalId = parentOrganizationUnitExternalId,
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            Path = Guard.AgainstNullOrWhiteSpace(path, nameof(path)),
            Depth = depth,
            IsActive = true
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
