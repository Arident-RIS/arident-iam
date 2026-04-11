using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Roles;

public sealed class RoleVersion : AuditableEntity
{
    private readonly List<RolePermissionGrant> _permissionGrants = [];

    private RoleVersion() { }

    public Guid RoleVersionExternalId { get; private set; }
    public Guid RoleDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public int VersionNumber { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset? PublishedAt { get; private set; }
    public VersionStatus Status { get; private set; }

    public IReadOnlyCollection<RolePermissionGrant> PermissionGrants => _permissionGrants.AsReadOnly();

    internal static RoleVersion Create(
        Guid roleDefinitionExternalId,
        Guid tenantExternalId,
        int versionNumber,
        string? description,
        string createdBy)
    {
        var entity = new RoleVersion
        {
            RoleVersionExternalId = Guid.NewGuid(),
            RoleDefinitionExternalId = Guard.AgainstDefault(roleDefinitionExternalId, nameof(roleDefinitionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            VersionNumber = versionNumber,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            Status = VersionStatus.Draft
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }

    internal void AddPermissionGrant(
        Guid permissionDefinitionExternalId,
        GrantType grantType,
        ScopeType scopeType,
        string? constraintJson,
        string createdBy)
    {
        if (Status != VersionStatus.Draft)
            throw new DomainException("Permission grants can only be modified on draft role versions.");

        var isDuplicate = _permissionGrants.Any(x =>
            x.PermissionDefinitionExternalId == permissionDefinitionExternalId &&
            x.GrantType == grantType &&
            x.ScopeType == scopeType);

        if (isDuplicate)
            throw new DomainException("Duplicate permission grant already exists in this role version.");

        _permissionGrants.Add(RolePermissionGrant.Create(
            roleVersionExternalId: RoleVersionExternalId,
            permissionDefinitionExternalId: permissionDefinitionExternalId,
            tenantExternalId: TenantExternalId,
            grantType: grantType,
            scopeType: scopeType,
            constraintJson: constraintJson,
            createdBy: createdBy));

        Touch(createdBy);
    }

    internal void Publish(string updatedBy)
    {
        if (Status != VersionStatus.Draft)
            throw new DomainException("Only draft role versions can be published.");

        Status = VersionStatus.Published;
        PublishedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    internal void Supersede(string updatedBy)
    {
        if (Status != VersionStatus.Published)
            throw new DomainException("Only published role versions can be superseded.");

        Status = VersionStatus.Superseded;
        Touch(updatedBy);
    }
}