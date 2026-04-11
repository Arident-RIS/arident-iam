using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyVersion : AuditableEntity
{
    private readonly List<PolicyTarget> _targets = [];
    private readonly List<PolicyRule> _rules = [];
    private readonly List<PolicyObligation> _obligations = [];

    private PolicyVersion() { }

    public Guid PolicyVersionExternalId { get; private set; }
    public Guid PolicyDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public int VersionNumber { get; private set; }
    public VersionStatus Status { get; private set; }
    public PolicyEffectType EffectType { get; private set; }
    public string CombiningAlgorithm { get; private set; } = null!;
    public DateTimeOffset? PublishedAt { get; private set; }
    public DateTimeOffset? EffectiveFrom { get; private set; }
    public DateTimeOffset? EffectiveTo { get; private set; }

    public IReadOnlyCollection<PolicyTarget> Targets => _targets.AsReadOnly();
    public IReadOnlyCollection<PolicyRule> Rules => _rules.AsReadOnly();
    public IReadOnlyCollection<PolicyObligation> Obligations => _obligations.AsReadOnly();

    internal static PolicyVersion Create(
        Guid policyDefinitionExternalId,
        Guid tenantExternalId,
        int versionNumber,
        PolicyEffectType effectType,
        string combiningAlgorithm,
        DateTimeOffset? effectiveFrom,
        DateTimeOffset? effectiveTo,
        string createdBy)
    {
        Guard.AgainstInvalidRange(effectiveFrom, effectiveTo, nameof(effectiveTo));

        var entity = new PolicyVersion
        {
            PolicyVersionExternalId = Guid.NewGuid(),
            PolicyDefinitionExternalId = Guard.AgainstDefault(policyDefinitionExternalId, nameof(policyDefinitionExternalId)),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            VersionNumber = versionNumber,
            Status = VersionStatus.Draft,
            EffectType = Guard.AgainstInvalidEnum(effectType, nameof(effectType)),
            CombiningAlgorithm = Guard.AgainstMaxLength(combiningAlgorithm, 100, nameof(combiningAlgorithm)),
            EffectiveFrom = effectiveFrom,
            EffectiveTo = effectiveTo
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }

    internal PolicyRule AddRule(
        string name,
        int ruleOrder,
        PolicyEffectType effectType,
        string? description,
        string updatedBy)
    {
        EnsureDraft();

        if (_rules.Any(x => x.RuleOrder == ruleOrder))
            throw new DomainException("A policy rule with the same order already exists.");

        var newRule = PolicyRule.Create(PolicyVersionExternalId, TenantExternalId, name, ruleOrder, effectType, description, updatedBy);
        _rules.Add(newRule);
        Touch(updatedBy);
        return newRule;
    }

    internal void AddTarget(
        string targetType,
        Guid? targetReferenceId,
        string? selectorExpression,
        string inclusionMode,
        string updatedBy)
    {
        EnsureDraft();
        _targets.Add(PolicyTarget.Create(PolicyVersionExternalId, TenantExternalId, targetType, targetReferenceId, selectorExpression, inclusionMode, updatedBy));
        Touch(updatedBy);
    }

    internal void AddObligation(
        string obligationType,
        string? parametersJson,
        int executionOrder,
        string updatedBy)
    {
        EnsureDraft();

        if (_obligations.Any(x => x.ExecutionOrder == executionOrder))
            throw new DomainException("A policy obligation with the same execution order already exists.");

        _obligations.Add(PolicyObligation.Create(PolicyVersionExternalId, TenantExternalId, obligationType, parametersJson, executionOrder, updatedBy));
        Touch(updatedBy);
    }

    internal void Publish(string updatedBy)
    {
        EnsureDraft();

        Status = VersionStatus.Published;
        PublishedAt = DateTimeOffset.UtcNow;
        Touch(updatedBy);
    }

    internal void Supersede(string updatedBy)
    {
        if (Status != VersionStatus.Published)
            throw new DomainException("Only published policy versions can be superseded.");

        Status = VersionStatus.Superseded;
        Touch(updatedBy);
    }

    private void EnsureDraft()
    {
        if (Status != VersionStatus.Draft)
            throw new DomainException("Only draft policy versions can be modified.");
    }
}