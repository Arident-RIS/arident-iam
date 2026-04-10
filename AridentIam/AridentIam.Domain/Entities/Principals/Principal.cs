using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Principals;

public sealed class Principal : AggregateRoot
{
    private Principal() { }
    public Guid PrincipalExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public PrincipalType PrincipalType { get; private set; }
    public string DisplayName { get; private set; } = null!;
    public string? ExternalReference { get; private set; }
    public PrincipalStatus Status { get; private set; }
    public LifecycleState LifecycleState { get; private set; }

    public static Principal Create(Guid tenantExternalId, PrincipalType principalType, string displayName, string? externalReference, string createdBy)
    {
        var entity = new Principal
        {
            PrincipalExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            PrincipalType = principalType,
            DisplayName = Guard.AgainstNullOrWhiteSpace(displayName, nameof(displayName)),
            ExternalReference = string.IsNullOrWhiteSpace(externalReference) ? null : externalReference.Trim(),
            Status = PrincipalStatus.Active,
            LifecycleState = LifecycleState.Active
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
