using AridentIam.Domain.Common;

namespace AridentIam.Domain.Entities.Tenants;

public sealed class TenantSetting : AuditableEntity
{
    private TenantSetting() { }
    public Guid TenantSettingExternalId { get; private set; }
    public Guid TenantExternalId { get; private set; }
    public string SettingKey { get; private set; } = null!;
    public string SettingValue { get; private set; } = null!;
    public string Category { get; private set; } = null!;
    public bool IsSensitive { get; private set; }

    public static TenantSetting Create(Guid tenantExternalId, string settingKey, string settingValue, string category, bool isSensitive, string createdBy)
    {
        var entity = new TenantSetting
        {
            TenantSettingExternalId = Guid.NewGuid(),
            TenantExternalId = Guard.AgainstDefault(tenantExternalId, nameof(tenantExternalId)),
            SettingKey = Guard.AgainstNullOrWhiteSpace(settingKey, nameof(settingKey)),
            SettingValue = Guard.AgainstNullOrWhiteSpace(settingValue, nameof(settingValue)),
            Category = Guard.AgainstNullOrWhiteSpace(category, nameof(category)),
            IsSensitive = isSensitive
        };
        entity.SetCreationAudit(createdBy);
        return entity;
    }
}
