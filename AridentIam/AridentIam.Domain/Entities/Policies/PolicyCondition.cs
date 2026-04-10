using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyCondition : AuditableEntity
{
    private PolicyCondition() { }
    public Guid PolicyConditionExternalId { get; private set; }
    public Guid PolicyRuleExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string ConditionGroup { get; private set; } = null!;
    public string LeftOperand { get; private set; } = null!;
    public string Operator { get; private set; } = null!;
    public string RightOperand { get; private set; } = null!;
    public string ValueType { get; private set; } = null!;
    public string LogicalJoin { get; private set; } = null!;
    public int EvaluationOrder { get; private set; }

    public static PolicyCondition Create(Guid policyRuleExternalId, Guid tenantExternalId, string conditionGroup, string leftOperand, string @operator, string rightOperand, string valueType, string logicalJoin, int evaluationOrder, string createdBy)
    {
        var entity = new PolicyCondition
        {
            PolicyConditionExternalId = Guid.NewGuid(),
            PolicyRuleExternalId = Guard.AgainstDefault(policyRuleExternalId, nameof(policyRuleExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            ConditionGroup = Guard.AgainstNullOrWhiteSpace(conditionGroup, nameof(conditionGroup)),
            LeftOperand = Guard.AgainstNullOrWhiteSpace(leftOperand, nameof(leftOperand)),
            Operator = Guard.AgainstNullOrWhiteSpace(@operator, nameof(@operator)),
            RightOperand = Guard.AgainstNullOrWhiteSpace(rightOperand, nameof(rightOperand)),
            ValueType = Guard.AgainstNullOrWhiteSpace(valueType, nameof(valueType)),
            LogicalJoin = Guard.AgainstNullOrWhiteSpace(logicalJoin, nameof(logicalJoin)),
            EvaluationOrder = evaluationOrder
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
