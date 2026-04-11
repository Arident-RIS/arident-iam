using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyRule : AuditableEntity
{
    private readonly List<PolicyCondition> _conditions = [];

    private PolicyRule() { }

    public Guid PolicyRuleExternalId { get; private set; }
    public Guid PolicyVersionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Name { get; private set; } = null!;
    public int RuleOrder { get; private set; }
    public PolicyEffectType EffectType { get; private set; }
    public bool IsActive { get; private set; }
    public string? Description { get; private set; }

    public IReadOnlyCollection<PolicyCondition> Conditions => _conditions.AsReadOnly();

    internal static PolicyRule Create(
        Guid policyVersionExternalId,
        Guid tenantExternalId,
        string name,
        int ruleOrder,
        PolicyEffectType effectType,
        string? description,
        string createdBy)
    {
        var entity = new PolicyRule
        {
            PolicyRuleExternalId = Guid.NewGuid(),
            PolicyVersionExternalId = Guard.AgainstDefault(policyVersionExternalId, nameof(policyVersionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstMaxLength(name, 200, nameof(name)),
            RuleOrder = Guard.AgainstNegative(ruleOrder, nameof(ruleOrder)),
            EffectType = Guard.AgainstInvalidEnum(effectType, nameof(effectType)),
            IsActive = true,
            Description = string.IsNullOrWhiteSpace(description) ? null : Guard.AgainstMaxLength(description, 1000, nameof(description))
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }

    internal void AddCondition(
        string conditionGroup,
        string leftOperand,
        string @operator,
        string rightOperand,
        string valueType,
        string logicalJoin,
        int evaluationOrder,
        string updatedBy)
    {
        _conditions.Add(PolicyCondition.Create(
            policyRuleExternalId: PolicyRuleExternalId,
            tenantExternalId: TenantExternalId,
            conditionGroup: conditionGroup,
            leftOperand: leftOperand,
            @operator: @operator,
            rightOperand: rightOperand,
            valueType: valueType,
            logicalJoin: logicalJoin,
            evaluationOrder: evaluationOrder,
            createdBy: updatedBy));

        Touch(updatedBy);
    }
}