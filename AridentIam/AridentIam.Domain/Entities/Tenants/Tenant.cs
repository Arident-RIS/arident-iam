using AridentIam.Domain.Common;
using AridentIam.Domain.Enums;

namespace AridentIam.Domain.Entities.Tenants;

public sealed class Tenant : AggregateRoot
{
    private readonly List<TenantSetting> _settings = [];

    private Tenant() { }

    public Guid TenantExternalId { get; private set; }
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public TenantStatus Status { get; private set; }
    public IsolationMode IsolationMode { get; private set; }
    public string DefaultTimeZone { get; private set; } = "UTC";
    public string DefaultLocale { get; private set; } = "en-US";
    public string? DataResidencyRegion { get; private set; }

    public IReadOnlyCollection<TenantSetting> Settings => _settings.AsReadOnly();

    public static Tenant Create(
        Guid tenantExternalId,
        string code,
        string name,
        IsolationMode isolationMode,
        string createdBy)
    {
        var entity = new Tenant
        {
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            Code = Guard.AgainstMaxLength(code, 100, nameof(code)).ToUpperInvariant(),
            Name = Guard.AgainstMaxLength(name, 200, nameof(name)),
            IsolationMode = Guard.AgainstInvalidEnum(isolationMode, nameof(isolationMode)),
            Status = TenantStatus.Active
        };

        entity.SetCreationAudit(createdBy);
        return entity;
    }

    public void Rename(string name, string updatedBy)
    {
        EnsureNotDeleted();
        Name = Guard.AgainstMaxLength(name, 200, nameof(name));
        Touch(updatedBy);
    }

    public void SetLocale(string locale, string updatedBy)
    {
        EnsureNotDeleted();
        DefaultLocale = Guard.AgainstMaxLength(locale, 20, nameof(locale));
        Touch(updatedBy);
    }

    public void SetTimeZone(string timeZone, string updatedBy)
    {
        EnsureNotDeleted();
        DefaultTimeZone = Guard.AgainstMaxLength(timeZone, 100, nameof(timeZone));
        Touch(updatedBy);
    }

    public void SetDataResidencyRegion(string? region, string updatedBy)
    {
        EnsureNotDeleted();
        DataResidencyRegion = string.IsNullOrWhiteSpace(region) ? null : Guard.AgainstMaxLength(region, 100, nameof(region));
        Touch(updatedBy);
    }

    public void UpsertSetting(string settingKey, string settingValue, string category, bool isSensitive, string updatedBy)
    {
        EnsureNotDeleted();

        var normalizedKey = Guard.AgainstMaxLength(settingKey, 200, nameof(settingKey));
        var existingSetting = _settings.SingleOrDefault(x => x.SettingKey.Equals(normalizedKey, StringComparison.OrdinalIgnoreCase));

        if (existingSetting is null)
        {
            _settings.Add(TenantSetting.Create(TenantExternalId, normalizedKey, settingValue, category, isSensitive, updatedBy));
        }
        else
        {
            existingSetting.UpdateValue(settingValue, isSensitive, updatedBy);
        }

        Touch(updatedBy);
    }

    public void Activate(string updatedBy)
    {
        EnsureNotDeleted();
        Status = TenantStatus.Active;
        Touch(updatedBy);
    }

    public void Suspend(string updatedBy)
    {
        EnsureNotDeleted();
        Status = TenantStatus.Suspended;
        Touch(updatedBy);
    }

    public void Archive(string updatedBy)
    {
        EnsureNotDeleted();
        Status = TenantStatus.Archived;
        Touch(updatedBy);
    }

    public void Delete(string updatedBy)
    {
        if (Status == TenantStatus.Deleted)
            throw new DomainException("Tenant is already deleted.");
        Status = TenantStatus.Deleted;
        Touch(updatedBy);
    }

    private void EnsureNotDeleted()
    {
        if (Status == TenantStatus.Deleted)
            throw new DomainException("Deleted tenants cannot be modified.");
    }
}