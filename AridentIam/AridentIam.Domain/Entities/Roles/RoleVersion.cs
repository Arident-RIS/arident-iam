using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Roles;

public sealed class RoleVersion : AuditableEntity
{
    private RoleVersion() { }
    public Guid RoleVersionExternalId { get; private set; }
    public Guid RoleDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public int VersionNumber { get; private set; }
    public string? Description { get; private set; }
    public DateTimeOffset? PublishedAt { get; private set; }
    public VersionStatus Status { get; private set; }

    public static RoleVersion Create(Guid roleDefinitionExternalId, Guid tenantExternalId, int versionNumber, string? description, string createdBy)
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

    public void Publish(string updatedBy)
    {
        if (Status != VersionStatus.Draft)
            throw new DomainException("Only draft role versions can be published.");
        Status = VersionStatus.Published;
        PublishedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }
}
