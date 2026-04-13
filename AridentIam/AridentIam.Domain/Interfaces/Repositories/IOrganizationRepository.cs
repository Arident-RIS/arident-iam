using AridentIam.Domain.Entities.Organizations;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IOrganizationRepository
{
    Task<OrgSchema?> GetOrgSchemaByExternalIdAsync(Guid orgSchemaExternalId, CancellationToken cancellationToken = default);

    Task<bool> ExistsOrgSchemaByNameAsync(Guid tenantExternalId, string name, CancellationToken cancellationToken = default);

    Task AddOrgSchemaAsync(OrgSchema orgSchema, CancellationToken cancellationToken = default);

    Task<OrgUnitType?> GetOrgUnitTypeByExternalIdAsync(Guid orgUnitTypeExternalId, CancellationToken cancellationToken = default);

    Task<bool> ExistsOrgUnitTypeByCodeAsync(Guid tenantExternalId, Guid orgSchemaExternalId, string code, CancellationToken cancellationToken = default);

    Task AddOrgUnitTypeAsync(OrgUnitType orgUnitType, CancellationToken cancellationToken = default);

    Task<OrgUnit?> GetOrgUnitByExternalIdAsync(Guid organizationUnitExternalId, CancellationToken cancellationToken = default);

    Task<bool> ExistsOrgUnitByCodeAsync(Guid tenantExternalId, Guid orgSchemaExternalId, string code, CancellationToken cancellationToken = default);

    Task AddOrgUnitAsync(OrgUnit orgUnit, CancellationToken cancellationToken = default);
}