using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Governance;

public sealed class SoDRule : AggregateRoot
{
    private SoDRule() { }
    public Guid SoDRuleExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public ConflictType ConflictType { get; private set; }
    public string RuleDefinitionJson { get; private set; } = null!;
    public SeverityLevel Severity { get; private set; }
    public bool IsActive { get; private set; }

    public static SoDRule Create(Guid tenantExternalId, string code, string name, ConflictType conflictType, string ruleDefinitionJson, SeverityLevel severity, string? description, string createdBy)
    {
        var entity = new SoDRule
        {
            SoDRuleExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            ConflictType = conflictType,
            RuleDefinitionJson = Guard.AgainstNullOrWhiteSpace(ruleDefinitionJson, nameof(ruleDefinitionJson)),
            Severity = severity,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            IsActive = true
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Activate(string updatedBy)
    {
        if (IsActive) throw new DomainException("SoD rule is already active.");
        IsActive = true;
        Touch(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        if (!IsActive) throw new DomainException("SoD rule is already inactive.");
        IsActive = false;
        Touch(updatedBy);
    }

    public void UpdateDefinition(string ruleDefinitionJson, string updatedBy)
    {
        RuleDefinitionJson = Guard.AgainstNullOrWhiteSpace(ruleDefinitionJson, nameof(ruleDefinitionJson));
        Touch(updatedBy);
    }
}