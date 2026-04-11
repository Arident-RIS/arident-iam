namespace AridentIam.Application.Features.Tenants.Commands.CreateTenant;

public sealed record CreateTenantResponse(
    Guid TenantExternalId,
    string Code,
    string Name);