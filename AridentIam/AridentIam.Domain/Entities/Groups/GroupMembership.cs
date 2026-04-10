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
    public DateTime? EffectiveFromUtc { get; private set; }
    public DateTime? EffectiveToUtc { get; private set; }

    public static GroupMembership Create(Guid groupExternalId, Guid principalExternalId, Guid tenantExternalId, string membershipSource, DateTime? effectiveFromUtc, DateTime? effectiveToUtc, string createdBy)
    {
        Guard.AgainstInvalidRange(effectiveFromUtc, effectiveToUtc, nameof(effectiveToUtc));
        var entity = new GroupMembership
        {
            GroupMembershipExternalId = Guid.NewGuid(),
            GroupExternalId = Guard.AgainstDefault(groupExternalId, nameof(groupExternalId)),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            MembershipSource = Guard.AgainstNullOrWhiteSpace(membershipSource, nameof(membershipSource)),
            EffectiveFromUtc = effectiveFromUtc,
            EffectiveToUtc = effectiveToUtc
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
