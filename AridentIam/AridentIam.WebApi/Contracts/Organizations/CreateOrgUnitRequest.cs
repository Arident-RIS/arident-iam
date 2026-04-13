namespace AridentIam.WebApi.Contracts.Organizations;

public sealed record CreateOrgUnitRequest(
    Guid TenantExternalId,
    Guid OrgSchemaExternalId,
    Guid OrgUnitTypeExternalId,
    Guid? ParentOrganizationUnitExternalId,
    string Code,
    string Name);