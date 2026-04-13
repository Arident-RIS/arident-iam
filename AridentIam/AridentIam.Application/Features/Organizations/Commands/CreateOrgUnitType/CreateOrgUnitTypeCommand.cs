using AridentIam.Application.Common.CQRS;

namespace AridentIam.Application.Features.Organizations.Commands.CreateOrgUnitType;

public sealed record CreateOrgUnitTypeCommand(
    Guid TenantExternalId,
    Guid OrgSchemaExternalId,
    string Code,
    string Name,
    int HierarchyLevel)
    : ICommand<CreateOrgUnitTypeResponse>;