namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgUnitType;

public sealed record CreateOrgUnitTypeResponse(
    Guid OrgUnitTypeExternalId,
    Guid OrgSchemaExternalId,
    string Code,
    string Name);