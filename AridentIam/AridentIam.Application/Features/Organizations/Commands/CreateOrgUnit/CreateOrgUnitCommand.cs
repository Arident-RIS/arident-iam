using AridentIam.Application.Common.CQRS;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgUnit;

public sealed record CreateOrgUnitCommand(
    Guid TenantExternalId,
    Guid OrgSchemaExternalId,
    Guid OrgUnitTypeExternalId,
    Guid? ParentOrganizationUnitExternalId,
    string Code,
    string Name)
    : ICommand<CreateOrgUnitResponse>;