using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Resources;

public sealed class ResourceType : AggregateRoot
{
    private ResourceType() { }
    public Guid ResourceTypeExternalId { get; private set; }
    public Guid? TenantExternalId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsGlobal { get; private set; }
    public bool SupportsInstanceLevelControl { get; private set; }
    public bool SupportsFieldLevelMasking { get; private set; }

    public static ResourceType Create(Guid? tenantExternalId, string code, string name, string? description, bool isGlobal, bool supportsInstanceLevelControl, bool supportsFieldLevelMasking, string createdBy)
    {
        var entity = new ResourceType
        {
            ResourceTypeExternalId = Guid.NewGuid(),
            TenantExternalId = tenantExternalId,
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
            IsGlobal = isGlobal,
            SupportsInstanceLevelControl = supportsInstanceLevelControl,
            SupportsFieldLevelMasking = supportsFieldLevelMasking
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
