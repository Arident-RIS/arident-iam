using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Roles;

public sealed class RoleDefinition : AggregateRoot
{
    private readonly List<RoleVersion> _versions = [];

    private RoleDefinition() { }

    public Guid RoleDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public string? Description { get; private set; }
    public string? RoleCategory { get; private set; }
    public bool IsSystemRole { get; private set; }
    public bool IsPrivileged { get; private set; }
    public RoleStatus Status { get; private set; }
    public int CurrentVersionNumber { get; private set; }

    public IReadOnlyCollection<RoleVersion> Versions => _versions.AsReadOnly();

    public static RoleDefinition Create(
        Guid tenantExternalId,
        string name,
        string code,
        string? description,
        string? roleCategory,
        bool isSystemRole,
        bool isPrivileged,
        string createdBy)
    {
        var entity = new RoleDefinition
        {
            RoleDefinitionExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstMaxLength(name, 200, nameof(name)),
            Code = Guard.AgainstMaxLength(code, 100, nameof(code)).ToUpperInvariant(),
            Description = string.IsNullOrWhiteSpace(description) ? null : Guard.AgainstMaxLength(description, 1000, nameof(description)),
            RoleCategory = string.IsNullOrWhiteSpace(roleCategory) ? null : Guard.AgainstMaxLength(roleCategory, 100, nameof(roleCategory)),
            IsSystemRole = isSystemRole,
            IsPrivileged = isPrivileged,
            Status = RoleStatus.Draft,
            CurrentVersionNumber = 1
        };

        entity.SetCreationAudit(createdBy);
        entity._versions.Add(RoleVersion.Create(entity.RoleDefinitionExternalId, entity.TenantExternalId, 1, description, createdBy));
        return entity;
    }

    public RoleVersion CreateDraftVersion(string? description, string updatedBy)
    {
        var existingDraft = _versions.SingleOrDefault(x => x.Status == VersionStatus.Draft);
        if (existingDraft is not null)
            throw new DomainException("A draft role version already exists.");

        CurrentVersionNumber++;
        var newVersion = RoleVersion.Create(RoleDefinitionExternalId, TenantExternalId, CurrentVersionNumber, description, updatedBy);
        _versions.Add(newVersion);
        Touch(updatedBy);
        return newVersion;
    }

    public void AddPermissionGrantToDraft(
        Guid permissionDefinitionExternalId,
        GrantType grantType,
        ScopeType scopeType,
        string? constraintJson,
        string updatedBy)
    {
        var draft = GetDraftVersion();
        draft.AddPermissionGrant(permissionDefinitionExternalId, grantType, scopeType, constraintJson, updatedBy);
        Touch(updatedBy);
    }

    public void PublishDraftVersion(string updatedBy)
    {
        var draft = GetDraftVersion();

        var publishedVersions = _versions.Where(x => x.Status == VersionStatus.Published).ToList();
        foreach (var version in publishedVersions)
            version.Supersede(updatedBy);

        draft.Publish(updatedBy);
        Status = RoleStatus.Active;
        Touch(updatedBy);
    }

    public void Deprecate(string updatedBy)
    {
        if (Status != RoleStatus.Active)
            throw new DomainException("Only active roles can be deprecated.");

        Status = RoleStatus.Deprecated;
        Touch(updatedBy);
    }

    public void Archive(string updatedBy)
    {
        if (Status == RoleStatus.Archived)
            return;

        Status = RoleStatus.Archived;
        Touch(updatedBy);
    }

    private RoleVersion GetDraftVersion()
    {
        return _versions.SingleOrDefault(x => x.Status == VersionStatus.Draft)
               ?? throw new DomainException("Draft role version was not found.");
    }
}