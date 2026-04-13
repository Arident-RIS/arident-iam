namespace AridentIam.WebApi.Contracts.Organizations;

public sealed record CreateOrgUnitTypeRequest(
    Guid TenantExternalId,
    Guid OrgSchemaExternalId,
    string Code,
    string Name,
    int HierarchyLevel);