using AridentIam.Domain.Entities.Organizations;
using AridentIam.Domain.Interfaces.Repositories;
using AridentIam.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AridentIam.Infrastructure.Persistence.Repositories.Organizations;

public sealed class OrganizationRepository(AridentIamDbContext dbContext) : IOrganizationRepository
{
    public async Task<OrgSchema?> GetOrgSchemaByExternalIdAsync(
        Guid orgSchemaExternalId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.OrgSchemas
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OrgSchemaExternalId == orgSchemaExternalId, cancellationToken);
    }

    public async Task<bool> ExistsOrgSchemaByNameAsync(
        Guid tenantExternalId,
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim();

        return await dbContext.OrgSchemas
            .AsNoTracking()
            .AnyAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.Name == normalizedName,
                cancellationToken);
    }

    public async Task AddOrgSchemaAsync(OrgSchema orgSchema, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orgSchema);
        await dbContext.OrgSchemas.AddAsync(orgSchema, cancellationToken);
    }

    public async Task<OrgUnitType?> GetOrgUnitTypeByExternalIdAsync(
        Guid orgUnitTypeExternalId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.OrgUnitTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OrgUnitTypeExternalId == orgUnitTypeExternalId, cancellationToken);
    }

    public async Task<bool> ExistsOrgUnitTypeByCodeAsync(
        Guid tenantExternalId,
        Guid orgSchemaExternalId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();

        return await dbContext.OrgUnitTypes
            .AsNoTracking()
            .AnyAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.OrgSchemaExternalId == orgSchemaExternalId &&
                     x.Code == normalizedCode,
                cancellationToken);
    }

    public async Task AddOrgUnitTypeAsync(OrgUnitType orgUnitType, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orgUnitType);
        await dbContext.OrgUnitTypes.AddAsync(orgUnitType, cancellationToken);
    }

    public async Task<OrgUnit?> GetOrgUnitByExternalIdAsync(
        Guid organizationUnitExternalId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.OrgUnits
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.OrganizationUnitExternalId == organizationUnitExternalId, cancellationToken);
    }

    public async Task<bool> ExistsOrgUnitByCodeAsync(
        Guid tenantExternalId,
        Guid orgSchemaExternalId,
        string code,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();

        return await dbContext.OrgUnits
            .AsNoTracking()
            .AnyAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.OrgSchemaExternalId == orgSchemaExternalId &&
                     x.Code == normalizedCode,
                cancellationToken);
    }

    public async Task AddOrgUnitAsync(OrgUnit orgUnit, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(orgUnit);
        await dbContext.OrgUnits.AddAsync(orgUnit, cancellationToken);
    }
}