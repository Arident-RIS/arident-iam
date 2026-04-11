using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Principals;

public sealed class PrincipalTenantMembership : AuditableEntity
{
    private PrincipalTenantMembership() { }
    public Guid PrincipalTenantMembershipExternalId { get; private set; }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string MembershipType { get; private set; } = null!;
    public string Status { get; private set; } = null!;
    public DateTimeOffset JoinedAt { get; private set; }
    public DateTimeOffset? LeftAt { get; private set; }

    public static PrincipalTenantMembership Create(Guid principalExternalId, Guid tenantExternalId, string membershipType, string status, string createdBy)
    {
        var entity = new PrincipalTenantMembership
        {
            PrincipalTenantMembershipExternalId = Guid.NewGuid(),
            PrincipalExternalId = Guard.AgainstDefault(principalExternalId, nameof(principalExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            MembershipType = Guard.AgainstNullOrWhiteSpace(membershipType, nameof(membershipType)),
            Status = Guard.AgainstNullOrWhiteSpace(status, nameof(status)),
            JoinedAt = DateTimeOffset.UtcNow
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
