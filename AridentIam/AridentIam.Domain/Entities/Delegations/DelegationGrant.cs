using AridentIam.Domain.Common;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Delegations;

public sealed class DelegationGrant : AggregateRoot
{
    private readonly List<DelegationScope> _scopes = [];

    private DelegationGrant() { }

    public Guid DelegationGrantExternalId { get; private set; }
    public Guid DelegationDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid? GrantedToPrincipalExternalId { get; private set; }
    public Guid? GrantedToGroupExternalId { get; private set; }
    public string GrantCeiling { get; private set; } = null!;
    public Guid? ApprovalRequestExternalId { get; private set; }
    public DateTimeOffset EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveTo { get; private set; }
    public bool IsActive { get; private set; }

    public IReadOnlyCollection<DelegationScope> Scopes => _scopes.AsReadOnly();

    public static DelegationGrant Create(Guid tenantExternalId, Guid delegationDefinitionExternalId, Guid? grantedToPrincipalExternalId, Guid? grantedToGroupExternalId, string grantCeiling, DateTimeOffset effectiveFrom, DateTimeOffset? effectiveTo, Guid? approvalRequestExternalId, string createdBy)
    {
        EnsureSingleTarget(grantedToPrincipalExternalId, grantedToGroupExternalId);
        Guard.AgainstInvalidRange(effectiveFrom, effectiveTo, nameof(effectiveTo));

        var entity = new DelegationGrant
        {
            DelegationGrantExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            DelegationDefinitionExternalId = Guard.AgainstDefault(delegationDefinitionExternalId, nameof(delegationDefinitionExternalId)),
            GrantedToPrincipalExternalId = grantedToPrincipalExternalId,
            GrantedToGroupExternalId = grantedToGroupExternalId,
            GrantCeiling = Guard.AgainstNullOrWhiteSpace(grantCeiling, nameof(grantCeiling)),
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo,
            ApprovalRequestExternalId = approvalRequestExternalId,
            IsActive = true
        };
        entity.SetCreationAudit(createdBy);
        entity.RaiseDomainEvent(new DelegationGrantedDomainEvent(entity.DelegationGrantExternalId, entity.DelegationDefinitionExternalId, entity.TenantExternalId));
        return entity;
    }

    public void AddScope(DelegationScope scope)
    {
        Guard.AgainstNull(scope, nameof(scope));
        _scopes.Add(scope);
    }

    public void Revoke(DateTimeOffset revokedAt, string updatedBy)
    {
        if (!IsActive)
            throw new DomainException("Delegation grant is already inactive.");
        if (revokedAt < EffectiveFrom)
            throw new DomainException("Delegation revoke date cannot be earlier than effective start date.");
        EffectiveTo = revokedAt;
        IsActive = false;
        Touch(updatedBy);
        RaiseDomainEvent(new DelegationRevokedDomainEvent(DelegationGrantExternalId, TenantExternalId));
    }

    public void Reinstate(string updatedBy)
    {
        if (IsActive)
            throw new DomainException("Delegation grant is already active.");
        if (EffectiveTo.HasValue && EffectiveTo.Value <= DateTimeOffset.UtcNow)
            throw new DomainException("Cannot reinstate an expired delegation grant.");
        IsActive = true;
        EffectiveTo = null;
        Touch(updatedBy);
    }

    public bool IsEffectiveAt(DateTimeOffset pointInTime) =>
        IsActive &&
        pointInTime >= EffectiveFrom &&
        (!EffectiveTo.HasValue || pointInTime <= EffectiveTo.Value);

    private static void EnsureSingleTarget(Guid? principalId, Guid? groupId)
    {
        var count = (principalId.HasValue ? 1 : 0) + (groupId.HasValue ? 1 : 0);
        if (count != 1)
            throw new DomainException("Delegation grant must target exactly one principal or one group.");
    }
}