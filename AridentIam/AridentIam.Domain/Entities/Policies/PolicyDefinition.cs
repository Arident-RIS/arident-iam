using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Policies;

public sealed class PolicyDefinition : AggregateRoot
{
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

    public static PolicyDefinition Create(Guid tenantExternalId, string name, string code, string? description, PolicyType policyType, int priority, bool isSystemManaged, string createdBy)
    {
        var entity = new PolicyDefinition
        {
            PolicyDefinitionExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            PolicyType = policyType,
            Priority = priority,
            Status = PolicyStatus.Draft,
            CurrentVersionNumber = 1,
            IsSystemManaged = isSystemManaged
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Activate(string updatedBy)
    {
        if (Status != PolicyStatus.Draft)
            throw new DomainException("Only draft policies can be activated.");
        Status = PolicyStatus.Active;
        Touch(updatedBy);
    }

    public void Deprecate(string updatedBy)
    {
        if (Status != PolicyStatus.Active)
            throw new DomainException("Only active policies can be deprecated.");
        Status = PolicyStatus.Deprecated;
        Touch(updatedBy);
    }

    public void IncrementVersion(string updatedBy)
    {
        CurrentVersionNumber++;
        Touch(updatedBy);
    }
}
