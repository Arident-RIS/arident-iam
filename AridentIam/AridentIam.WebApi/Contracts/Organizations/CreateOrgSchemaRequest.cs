namespace AridentIam.WebApi.Contracts.Organizations;

public sealed record CreateOrgSchemaRequest(
    Guid TenantExternalId,
    string Name,
    string? Description,
    bool IsDefault);