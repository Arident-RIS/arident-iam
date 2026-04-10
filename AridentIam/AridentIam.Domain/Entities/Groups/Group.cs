using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Groups;

public sealed class Group : AggregateRoot
{
    private Group() { }
    public Guid GroupExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public string? Description { get; private set; }
    public GroupType GroupType { get; private set; }
    public Guid? ParentGroupExternalId { get; private set; }
    public bool IsActive { get; private set; }

    public static Group Create(Guid tenantExternalId, string name, string code, string? description, GroupType groupType, Guid? parentGroupExternalId, string createdBy)
    {
        var entity = new Group
        {
            GroupExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            GroupType = groupType,
            ParentGroupExternalId = parentGroupExternalId,
            IsActive = true
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
