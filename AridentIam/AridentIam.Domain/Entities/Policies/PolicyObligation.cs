using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyObligation : AuditableEntity
{
    private PolicyObligation() { }
    public Guid PolicyObligationExternalId { get; private set; }
    public Guid PolicyVersionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string ObligationType { get; private set; } = null!;
    public string? ParametersJson { get; private set; }
    public int ExecutionOrder { get; private set; }

    public static PolicyObligation Create(Guid policyVersionExternalId, Guid tenantExternalId, string obligationType, string? parametersJson, int executionOrder, string createdBy)
    {
        var entity = new PolicyObligation
        {
            PolicyObligationExternalId = Guid.NewGuid(),
            PolicyVersionExternalId = Guard.AgainstDefault(policyVersionExternalId, nameof(policyVersionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            ObligationType = Guard.AgainstNullOrWhiteSpace(obligationType, nameof(obligationType)),
            ParametersJson = string.IsNullOrWhiteSpace(parametersJson) ? null : parametersJson.Trim(),
            ExecutionOrder = executionOrder
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
