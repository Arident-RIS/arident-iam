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

    internal static PolicyCondition Create(
        Guid policyRuleExternalId,
        Guid tenantExternalId,
        string conditionGroup,
        string leftOperand,
        string @operator,
        string rightOperand,
        string valueType,
        string logicalJoin,
        int evaluationOrder,
        string createdBy)
    {
        var entity = new PolicyCondition
        {
            PolicyConditionExternalId = Guid.NewGuid(),
            PolicyRuleExternalId = Guard.AgainstDefault(policyRuleExternalId, nameof(policyRuleExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            ConditionGroup = Guard.AgainstMaxLength(conditionGroup, 100, nameof(conditionGroup)),
            LeftOperand = Guard.AgainstMaxLength(leftOperand, 200, nameof(leftOperand)),
            Operator = Guard.AgainstMaxLength(@operator, 50, nameof(@operator)),
            RightOperand = Guard.AgainstMaxLength(rightOperand, 500, nameof(rightOperand)),
            ValueType = Guard.AgainstMaxLength(valueType, 50, nameof(valueType)),
            LogicalJoin = Guard.AgainstMaxLength(logicalJoin, 20, nameof(logicalJoin)),
            EvaluationOrder = Guard.AgainstNegative(evaluationOrder, nameof(evaluationOrder))
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }
}