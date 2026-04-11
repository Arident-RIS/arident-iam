using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Groups;

public sealed class GroupMembership : AuditableEntity
{
    private GroupMembership() { }
    public Guid GroupMembershipExternalId { get; private set; }
    public Guid GroupExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string MembershipSource { get; private set; } = null!;
    public DateTimeOffset? EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveTo { get; private set; }

    public static GroupMembership Create(Guid groupExternalId, Guid principalExternalId, Guid tenantExternalId, string membershipSource, DateTimeOffset? effectiveFrom, DateTimeOffset? effectiveTo, string createdBy)
    {
        Guard.AgainstInvalidRange(effectiveFrom, effectiveTo, nameof(effectiveTo));
        var entity = new GroupMembership
        {
            GroupMembershipExternalId = Guid.NewGuid(),
            GroupExternalId = Guard.AgainstDefault(groupExternalId, nameof(groupExternalId)),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            MembershipSource = Guard.AgainstNullOrWhiteSpace(membershipSource, nameof(membershipSource)),
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
