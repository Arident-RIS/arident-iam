using AridentIam.Domain.Entities.Tenants;
using AridentIam.Domain.Interfaces.Repositories;
using AridentIam.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AridentIam.Infrastructure.Persistence.Repositories.Tenants;

public sealed class TenantRepository(AridentIamDbContext dbContext) : ITenantRepository
{
    public async Task<Tenant?> GetByExternalIdAsync(
        Guid tenantExternalId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.TenantExternalId == tenantExternalId,
                cancellationToken);
    }

    public async Task<bool> ExistsByExternalIdAsync(
        Guid tenantExternalId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Tenants
            .AsNoTracking()
            .AnyAsync(
                x => x.TenantExternalId == tenantExternalId,
                cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim();

        return await dbContext.Tenants
            .AsNoTracking()
            .AnyAsync(
                x => x.Code == normalizedCode,
                cancellationToken);
    }

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        await dbContext.Tenants.AddAsync(tenant, cancellationToken);
    }

    public void Update(Tenant tenant)
    {
        ArgumentNullException.ThrowIfNull(tenant);

        dbContext.Tenants.Update(tenant);
    }
}