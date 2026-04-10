using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Roles;

public sealed class RoleAssignmentScope : AuditableEntity
{
    private RoleAssignmentScope() { }
    public Guid RoleAssignmentScopeExternalId { get; private set; }
    public Guid RoleAssignmentExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public ScopeType ScopeType { get; private set; }
    public Guid? OrganizationUnitExternalId { get; private set; }
    public Guid? ResourceTypeExternalId { get; private set; }
    public string? ResourceInstanceKey { get; private set; }
    public string? ScopeValue { get; private set; }

    public static RoleAssignmentScope Create(Guid roleAssignmentExternalId, Guid tenantExternalId, ScopeType scopeType, Guid? organizationUnitExternalId, Guid? resourceTypeExternalId, string? resourceInstanceKey, string? scopeValue, string createdBy)
    {
        var entity = new RoleAssignmentScope
        {
            RoleAssignmentScopeExternalId = Guid.NewGuid(),
            RoleAssignmentExternalId = Guard.AgainstDefault(roleAssignmentExternalId, nameof(roleAssignmentExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            ScopeType = scopeType,
            OrganizationUnitExternalId = organizationUnitExternalId,
            ResourceTypeExternalId = resourceTypeExternalId,
            ResourceInstanceKey = string.IsNullOrWhiteSpace(resourceInstanceKey) ? null : resourceInstanceKey.Trim(),
            ScopeValue = string.IsNullOrWhiteSpace(scopeValue) ? null : scopeValue.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
