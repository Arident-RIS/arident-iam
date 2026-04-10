using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Roles;

public sealed class RoleDefinition : AggregateRoot
{
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

    public static RoleDefinition Create(Guid tenantExternalId, string name, string code, string? description, string? roleCategory, bool isSystemRole, bool isPrivileged, string createdBy)
    {
        var entity = new RoleDefinition
        {
            RoleDefinitionExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            RoleCategory = string.IsNullOrWhiteSpace(roleCategory) ? null : roleCategory.Trim(),
            IsSystemRole = isSystemRole,
            IsPrivileged = isPrivileged,
            Status = RoleStatus.Draft,
            CurrentVersionNumber = 1
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Activate(string updatedBy)
    {
        if (Status != RoleStatus.Draft)
            throw new DomainException("Only draft roles can be activated.");
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

    public void IncrementVersion(string updatedBy)
    {
        CurrentVersionNumber++;
        Touch(updatedBy);
    }
}
