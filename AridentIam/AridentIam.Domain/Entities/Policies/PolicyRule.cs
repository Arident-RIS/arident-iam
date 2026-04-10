using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyRule : AuditableEntity
{
    private PolicyRule() { }
    public Guid PolicyRuleExternalId { get; private set; }
    public Guid PolicyVersionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Name { get; private set; } = null!;
    public int RuleOrder { get; private set; }
    public PolicyEffectType EffectType { get; private set; }
    public bool IsActive { get; private set; }
    public string? Description { get; private set; }

    public static PolicyRule Create(Guid policyVersionExternalId, Guid tenantExternalId, string name, int ruleOrder, PolicyEffectType effectType, string? description, string createdBy)
    {
        var entity = new PolicyRule
        {
            PolicyRuleExternalId = Guid.NewGuid(),
            PolicyVersionExternalId = Guard.AgainstDefault(policyVersionExternalId, nameof(policyVersionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            RuleOrder = ruleOrder,
            EffectType = effectType,
            IsActive = true,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim()
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
