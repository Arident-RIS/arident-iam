using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyTarget : AuditableEntity
{
    private PolicyTarget() { }

    public Guid PolicyTargetExternalId { get; private set; }
    public Guid PolicyVersionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string TargetType { get; private set; } = null!;
    public Guid? TargetReferenceId { get; private set; }
    public string? SelectorExpression { get; private set; }
    public string InclusionMode { get; private set; } = null!;

    internal static PolicyTarget Create(
        Guid policyVersionExternalId,
        Guid tenantExternalId,
        string targetType,
        Guid? targetReferenceId,
        string? selectorExpression,
        string inclusionMode,
        string createdBy)
    {
        var entity = new PolicyTarget
        {
            PolicyTargetExternalId = Guid.NewGuid(),
            PolicyVersionExternalId = Guard.AgainstDefault(policyVersionExternalId, nameof(policyVersionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            TargetType = Guard.AgainstMaxLength(targetType, 100, nameof(targetType)),
            TargetReferenceId = targetReferenceId,
            SelectorExpression = string.IsNullOrWhiteSpace(selectorExpression) ? null : selectorExpression.Trim(),
            InclusionMode = Guard.AgainstMaxLength(inclusionMode, 50, nameof(inclusionMode))
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }
}