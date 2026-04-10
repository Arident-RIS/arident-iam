using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Delegations;

public sealed class DelegationScope : AuditableEntity
{
    private DelegationScope() { }
    public Guid DelegationScopeExternalId { get; private set; }
    public Guid DelegationGrantExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public ScopeType ScopeType { get; private set; }
    public Guid? OrganizationUnitExternalId { get; private set; }
    public Guid? RoleDefinitionExternalId { get; private set; }
    public Guid? ResourceTypeExternalId { get; private set; }
    public string? ScopeValue { get; private set; }

    public static DelegationScope Create(Guid delegationGrantExternalId, Guid tenantExternalId, ScopeType scopeType, Guid? organizationUnitExternalId, Guid? roleDefinitionExternalId, Guid? resourceTypeExternalId, string? scopeValue, string createdBy)
    {
        var entity = new DelegationScope
        {
            DelegationScopeExternalId = Guid.NewGuid(),
            DelegationGrantExternalId = Guard.AgainstDefault(delegationGrantExternalId, nameof(delegationGrantExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            ScopeType = scopeType,
            OrganizationUnitExternalId = organizationUnitExternalId,
            RoleDefinitionExternalId = roleDefinitionExternalId,
            ResourceTypeExternalId = resourceTypeExternalId,
            ScopeValue = string.IsNullOrWhiteSpace(scopeValue) ? null : scopeValue.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}