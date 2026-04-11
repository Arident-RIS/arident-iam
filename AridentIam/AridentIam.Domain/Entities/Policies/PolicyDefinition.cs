using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Events;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyDefinition : AggregateRoot
{
    private readonly List<PolicyVersion> _versions = [];

    private PolicyDefinition() { }

    public Guid PolicyDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Name { get; private set; } = null!;
    public string Code { get; private set; } = null!;
    public string? Description { get; private set; }
    public PolicyType PolicyType { get; private set; }
    public int Priority { get; private set; }
    public PolicyStatus Status { get; private set; }
    public int CurrentVersionNumber { get; private set; }
    public bool IsSystemManaged { get; private set; }

    public IReadOnlyCollection<PolicyVersion> Versions => _versions.AsReadOnly();

    public static PolicyDefinition Create(
        Guid tenantExternalId,
        string name,
        string code,
        string? description,
        PolicyType policyType,
        int priority,
        bool isSystemManaged,
        string createdBy)
    {
        var entity = new PolicyDefinition
        {
            PolicyDefinitionExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstMaxLength(name, 200, nameof(name)),
            Code = Guard.AgainstMaxLength(code, 100, nameof(code)).ToUpperInvariant(),
            Description = string.IsNullOrWhiteSpace(description) ? null : Guard.AgainstMaxLength(description, 1000, nameof(description)),
            PolicyType = Guard.AgainstInvalidEnum(policyType, nameof(policyType)),
            Priority = Guard.AgainstNegative(priority, nameof(priority)),
            Status = PolicyStatus.Draft,
            CurrentVersionNumber = 1,
            IsSystemManaged = isSystemManaged
        };

        entity.SetCreationAudit(createdBy);
        entity._versions.Add(PolicyVersion.Create(
            policyDefinitionExternalId: entity.PolicyDefinitionExternalId,
            tenantExternalId: entity.TenantExternalId,
            versionNumber: 1,
            effectType: PolicyEffectType.Deny,
            combiningAlgorithm: "deny-overrides",
            effectiveFrom: null,
            effectiveTo: null,
            createdBy: createdBy));

        return entity;
    }

    public PolicyVersion CreateDraftVersion(
        PolicyEffectType effectType,
        string combiningAlgorithm,
        DateTimeOffset? effectiveFrom,
        DateTimeOffset? effectiveTo,
        string updatedBy)
    {
        var existingDraft = _versions.SingleOrDefault(x => x.Status == VersionStatus.Draft);
        if (existingDraft is not null)
            throw new DomainException("A draft policy version already exists.");

        CurrentVersionNumber++;

        var newVersion = PolicyVersion.Create(
            policyDefinitionExternalId: PolicyDefinitionExternalId,
            tenantExternalId: TenantExternalId,
            versionNumber: CurrentVersionNumber,
            effectType: effectType,
            combiningAlgorithm: combiningAlgorithm,
            effectiveFrom: effectiveFrom,
            effectiveTo: effectiveTo,
            createdBy: updatedBy);

        _versions.Add(newVersion);
        Touch(updatedBy);
        return newVersion;
    }

    public PolicyRule AddRuleToDraft(
        string name,
        int ruleOrder,
        PolicyEffectType effectType,
        string? description,
        string updatedBy)
    {
        var draft = GetDraftVersion();
        var newRule = draft.AddRule(name, ruleOrder, effectType, description, updatedBy);
        Touch(updatedBy);
        return newRule;
    }

    public void AddConditionToDraftRule(
        Guid policyRuleExternalId,
        string conditionGroup,
        string leftOperand,
        string @operator,
        string rightOperand,
        string valueType,
        string logicalJoin,
        int evaluationOrder,
        string updatedBy)
    {
        var draft = GetDraftVersion();
        var rule = draft.Rules.SingleOrDefault(x => x.PolicyRuleExternalId == policyRuleExternalId)
                  ?? throw new DomainException("Policy rule does not belong to the current draft version.");

        rule.AddCondition(conditionGroup, leftOperand, @operator, rightOperand, valueType, logicalJoin, evaluationOrder, updatedBy);
        Touch(updatedBy);
    }

    public void AddTargetToDraft(
        string targetType,
        Guid? targetReferenceId,
        string? selectorExpression,
        string inclusionMode,
        string updatedBy)
    {
        var draft = GetDraftVersion();
        draft.AddTarget(targetType, targetReferenceId, selectorExpression, inclusionMode, updatedBy);
        Touch(updatedBy);
    }

    public void AddObligationToDraft(
        string obligationType,
        string? parametersJson,
        int executionOrder,
        string updatedBy)
    {
        var draft = GetDraftVersion();
        draft.AddObligation(obligationType, parametersJson, executionOrder, updatedBy);
        Touch(updatedBy);
    }

    public void PublishDraftVersion(string updatedBy)
    {
        var draft = GetDraftVersion();

        var publishedVersions = _versions.Where(x => x.Status == VersionStatus.Published).ToList();
        foreach (var version in publishedVersions)
            version.Supersede(updatedBy);

        draft.Publish(updatedBy);
        Status = PolicyStatus.Active;
        Touch(updatedBy);
        RaiseDomainEvent(new PolicyPublishedDomainEvent(draft.PolicyVersionExternalId));
    }

    public void Deprecate(string updatedBy)
    {
        if (Status != PolicyStatus.Active)
            throw new DomainException("Only active policies can be deprecated.");

        Status = PolicyStatus.Deprecated;
        Touch(updatedBy);
    }

    public void Archive(string updatedBy)
    {
        if (Status == PolicyStatus.Archived)
            return;

        Status = PolicyStatus.Archived;
        Touch(updatedBy);
    }

    private PolicyVersion GetDraftVersion()
    {
        return _versions.SingleOrDefault(x => x.Status == VersionStatus.Draft)
               ?? throw new DomainException("Draft policy version was not found.");
    }
}