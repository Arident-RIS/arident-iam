using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Roles;

public sealed class RoleAssignment : AggregateRoot
{
    private RoleAssignment() { }
    public Guid RoleAssignmentExternalId { get; private set; }
    public Guid RoleDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid? AssignedToPrincipalExternalId { get; private set; }
    public Guid? AssignedToGroupExternalId { get; private set; }
    public RoleAssignmentType AssignmentType { get; private set; }
    public LifecycleState Status { get; private set; }
    public DateTimeOffset? EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveTo { get; private set; }
    public Guid? ApprovalRequestExternalId { get; private set; }
    public string? AssignedReason { get; private set; }

    public static RoleAssignment Create(Guid roleDefinitionExternalId, Guid tenantExternalId, Guid? assignedToPrincipalExternalId, Guid? assignedToGroupExternalId, RoleAssignmentType assignmentType, DateTimeOffset? effectiveFrom, DateTimeOffset? effectiveTo, Guid? approvalRequestExternalId, string? assignedReason, string createdBy)
    {
        if (!assignedToPrincipalExternalId.HasValue && !assignedToGroupExternalId.HasValue)
            throw new DomainException("RoleAssignment requires a principal or group target.");

        Guard.AgainstInvalidRange(effectiveFrom, effectiveTo, nameof(effectiveTo));

        var entity = new RoleAssignment
        {
            RoleAssignmentExternalId = Guid.NewGuid(),
            RoleDefinitionExternalId = Guard.AgainstDefault(roleDefinitionExternalId, nameof(roleDefinitionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            AssignedToPrincipalExternalId = assignedToPrincipalExternalId,
            AssignedToGroupExternalId = assignedToGroupExternalId,
            AssignmentType = assignmentType,
            Status = LifecycleState.Active,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            ApprovalRequestExternalId = approvalRequestExternalId,
            AssignedReason = string.IsNullOrWhiteSpace(assignedReason) ? null : assignedReason.Trim()
        };
        entity.SetCreationAudit(createdBy);
        entity.RaiseDomainEvent(new RoleAssignedDomainEvent(entity.RoleAssignmentExternalId, entity.RoleDefinitionExternalId));
        return entity;
    }

    public void Revoke(string updatedBy)
    {
        if (Status == LifecycleState.Deprovisioned)
            throw new DomainException("Role assignment is already revoked.");
        Status = LifecycleState.Deprovisioned;
        Touch(updatedBy);
    }

    public void Expire(string updatedBy)
    {
        if (Status != LifecycleState.Active)
            throw new DomainException("Only active role assignments can be expired.");
        Status = LifecycleState.Disabled;
        Touch(updatedBy);
    }
}
