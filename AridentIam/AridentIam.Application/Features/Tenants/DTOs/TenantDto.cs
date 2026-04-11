using AridentIam.Domain.Enums;

namespace AridentIam.Application.Features.Tenants.DTOs;

public sealed record TenantDto(
    Guid TenantExternalId,
    string Code,
    string Name,
    TenantStatus Status,
    IsolationMode IsolationMode,
    string? DefaultLocale,
    string? DefaultTimeZone,
    string? DataResidencyRegion);