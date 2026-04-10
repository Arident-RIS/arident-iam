using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Principals;

public sealed class PrincipalOrgMembership : AuditableEntity
{
    private PrincipalOrgMembership() { }
    public Guid PrincipalOrgMembershipExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public Guid OrganizationUnitExternalId { get; private set; }
    public string MembershipRole { get; private set; } = null!;
    public bool IsPrimary { get; private set; }

    public static PrincipalOrgMembership Create(Guid principalExternalId, Guid tenantExternalId, Guid organizationUnitExternalId, string membershipRole, bool isPrimary, string createdBy)
    {
        var entity = new PrincipalOrgMembership
        {
            PrincipalOrgMembershipExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            OrganizationUnitExternalId = Guard.AgainstDefault(organizationUnitExternalId, nameof(organizationUnitExternalId)),
            MembershipRole = Guard.AgainstNullOrWhiteSpace(membershipRole, nameof(membershipRole)),
            IsPrimary = isPrimary
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
