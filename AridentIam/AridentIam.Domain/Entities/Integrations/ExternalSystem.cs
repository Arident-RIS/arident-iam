using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Integrations;

public sealed class ExternalSystem : AggregateRoot
{
    private ExternalSystem() { }
    public Guid ExternalSystemExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public bool IsSystemOwned => !TenantExternalId.HasValue;
    public string SystemName { get; private set; } = null!;
    public ExternalSystemType SystemType { get; private set; }
    public string? BaseReference { get; private set; }
    public bool IsActive { get; private set; }

    public static ExternalSystem Create(Guid? tenantExternalId, string systemName, ExternalSystemType systemType, string? baseReference, string createdBy)
    {
        var entity = new ExternalSystem
        {
            ExternalSystemExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            SystemName = Guard.AgainstNullOrWhiteSpace(systemName, nameof(systemName)),
            SystemType = systemType,
            BaseReference = string.IsNullOrWhiteSpace(baseReference) ? null : baseReference.Trim(),
            IsActive = true
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Disable(string updatedBy) { IsActive = false; Touch(updatedBy); }
    public void Enable(string updatedBy) { IsActive = true; Touch(updatedBy); }
}