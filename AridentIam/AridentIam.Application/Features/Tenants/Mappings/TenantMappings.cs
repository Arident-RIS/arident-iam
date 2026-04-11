using AridentIam.Application.Features.Tenants.DTOs;
using AridentIam.Domain.Entities.Tenants;

namespace AridentIam.Application.Features.Tenants.Mappings;

public static class TenantMappings
{
    public static TenantDto ToDto(this Tenant tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        return new TenantDto(
            TenantExternalId: tenant.TenantExternalId,
            Code: tenant.Code,
            Name: tenant.Name,
            Status: tenant.Status,
            IsolationMode: tenant.IsolationMode,
            DefaultLocale: tenant.DefaultLocale,
            DefaultTimeZone: tenant.DefaultTimeZone,
            DataResidencyRegion: tenant.DataResidencyRegion);
    }
}