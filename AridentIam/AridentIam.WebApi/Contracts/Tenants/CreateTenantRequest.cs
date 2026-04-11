using AridentIam.Domain.Enums;

namespace AridentIam.WebApi.Contracts.Tenants;

public sealed record CreateTenantRequest(
    string Code,
    string Name,
    IsolationMode IsolationMode,
    string? DefaultLocale,
    string? DefaultTimeZone);