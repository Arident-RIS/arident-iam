using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Delegations;

public sealed class DelegationDefinition : AggregateRoot
{
    private DelegationDefinition() { }
    public Guid DelegationDefinitionExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public DelegationType DelegationType { get; private set; }
    public bool IsActive { get; private set; }

    public static DelegationDefinition Create(Guid tenantExternalId, string code, string name, DelegationType delegationType, string? description, string createdBy)
    {
        var entity = new DelegationDefinition
        {
            DelegationDefinitionExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            DelegationType = delegationType,
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            IsActive = true
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Activate(string updatedBy)
    {
        if (IsActive) throw new DomainException("Delegation definition is already active.");
        IsActive = true;
        Touch(updatedBy);
    }

    public void Deactivate(string updatedBy)
    {
        if (!IsActive) throw new DomainException("Delegation definition is already inactive.");
        IsActive = false;
        Touch(updatedBy);
    }

    public void UpdateDetails(string name, string? description, string updatedBy)
    {
        Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name));
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        Touch(updatedBy);
    }
}