using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Tenants;

public sealed class Tenant : AggregateRoot
{
    private Tenant() { }
    public Guid TenantExternalId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public TenantStatus Status { get; private set; }
    public IsolationMode IsolationMode { get; private set; }
    public string DefaultTimeZone { get; private set; } = "UTC";
    public string DefaultLocale { get; private set; } = "en-US";
    public string? DataResidencyRegion { get; private set; }

    public static Tenant Create(Guid tenantExternalId, string code, string name, IsolationMode isolationMode, string createdBy)
    {
        var entity = new Tenant
        {
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Code = Guard.AgainstNullOrWhiteSpace(code, nameof(code)),
            Name = Guard.AgainstNullOrWhiteSpace(name, nameof(name)),
            IsolationMode = isolationMode,
            Status = TenantStatus.Active
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Activate(string updatedBy) { Status = TenantStatus.Active; Touch(updatedBy); }
    public void Suspend(string updatedBy) { Status = TenantStatus.Suspended; Touch(updatedBy); }
}
