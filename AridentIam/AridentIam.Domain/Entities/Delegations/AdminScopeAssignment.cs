using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Delegations;

public sealed class AdminScopeAssignment : AggregateRoot
{
    private AdminScopeAssignment() { }
    public Guid AdminScopeAssignmentExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public AdminCapability AdminCapability { get; private set; }
    public ScopeType BoundScopeType { get; private set; }
    public Guid? BoundScopeReferenceExternalId { get; private set; }
    public DateTimeOffset EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveTo { get; private set; }

    public static AdminScopeAssignment Create(Guid tenantExternalId, Guid principalExternalId, AdminCapability adminCapability, ScopeType boundScopeType, Guid? boundScopeReferenceExternalId, DateTimeOffset effectiveFrom, DateTimeOffset? effectiveTo, string createdBy)
    {
        Guard.AgainstInvalidRange(effectiveFrom, effectiveTo, nameof(effectiveTo));

        var entity = new AdminScopeAssignment
        {
            AdminScopeAssignmentExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            AdminCapability = adminCapability,
            BoundScopeType = boundScopeType,
            BoundScopeReferenceExternalId = boundScopeReferenceExternalId,
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Revoke(DateTimeOffset revokedAt, string updatedBy)
    {
        Guard.AgainstInvalidRange(EffectiveFrom, revokedAt, nameof(revokedAt));
        EffectiveTo = revokedAt;
        Touch(updatedBy);
    }

    public bool IsEffectiveAt(DateTimeOffset pointInTime) =>
        pointInTime >= EffectiveFrom &&
        (!EffectiveTo.HasValue || pointInTime <= EffectiveTo.Value);
}