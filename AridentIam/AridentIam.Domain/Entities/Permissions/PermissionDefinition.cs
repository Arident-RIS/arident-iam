using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Permissions;

public sealed class PermissionDefinition : AggregateRoot
{
    private PermissionDefinition() { }
    public Guid PermissionDefinitionExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public string PermissionKey { get; private set; } = null!;
    public Guid ResourceTypeExternalId { get; private set; }
    public Guid ResourceActionExternalId { get; private set; }
    public string? Description { get; private set; }
    public ScopeType ScopeType { get; private set; }
    public bool IsPrivileged { get; private set; }
    public string Status { get; private set; } = null!;
    public int VersionNumber { get; private set; }

    public static PermissionDefinition Create(Guid? tenantExternalId, string permissionKey, Guid resourceTypeExternalId, Guid resourceActionExternalId, string? description, ScopeType scopeType, bool isPrivileged, int versionNumber, string createdBy)
    {
        var entity = new PermissionDefinition
        {
            PermissionDefinitionExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            PermissionKey = Guard.AgainstNullOrWhiteSpace(permissionKey, nameof(permissionKey)),
            ResourceTypeExternalId = Guard.AgainstDefault(resourceTypeExternalId, nameof(resourceTypeExternalId)),
            ResourceActionExternalId = Guard.AgainstDefault(resourceActionExternalId, nameof(resourceActionExternalId)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            ScopeType = scopeType,
            IsPrivileged = isPrivileged,
            Status = "Active",
            VersionNumber = versionNumber
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
