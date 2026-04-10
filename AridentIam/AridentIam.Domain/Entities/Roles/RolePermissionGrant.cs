using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Roles;

public sealed class RolePermissionGrant : AuditableEntity
{
    private RolePermissionGrant() { }
    public Guid RolePermissionGrantExternalId { get; private set; }
    public Guid RoleVersionExternalId { get; private set; }
    public Guid PermissionDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public GrantType GrantType { get; private set; }
    public ScopeType ScopeType { get; private set; }
    public string? ConstraintJson { get; private set; }

    public static RolePermissionGrant Create(Guid roleVersionExternalId, Guid permissionDefinitionExternalId, Guid tenantExternalId, GrantType grantType, ScopeType scopeType, string? constraintJson, string createdBy)
    {
        var entity = new RolePermissionGrant
        {
            RolePermissionGrantExternalId = Guid.NewGuid(),
            RoleVersionExternalId = Guard.AgainstDefault(roleVersionExternalId, nameof(roleVersionExternalId)),
            PermissionDefinitionExternalId = Guard.AgainstDefault(permissionDefinitionExternalId, nameof(permissionDefinitionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            GrantType = grantType,
            ScopeType = scopeType,
            ConstraintJson = string.IsNullOrWhiteSpace(constraintJson) ? null : constraintJson.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
