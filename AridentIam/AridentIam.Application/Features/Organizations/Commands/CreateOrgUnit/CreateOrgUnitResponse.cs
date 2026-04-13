namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgUnit;

public sealed record CreateOrgUnitResponse(
    Guid OrganizationUnitExternalId,
    Guid OrgSchemaExternalId,
    string Code,
    string Name);