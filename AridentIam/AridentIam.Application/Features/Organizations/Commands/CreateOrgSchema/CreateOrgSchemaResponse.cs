namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgSchema;

public sealed record CreateOrgSchemaResponse(
    Guid OrgSchemaExternalId,
    Guid TenantExternalId,
    string Name);