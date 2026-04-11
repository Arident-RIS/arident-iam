using AridentIam.Domain.Entities.Tenants;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface ITenantRepository
{
    Task<Tenant?> GetByExternalIdAsync(Guid tenantExternalId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByExternalIdAsync(Guid tenantExternalId, CancellationToken cancellationToken = default);

    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);

    Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default);

    void Update(Tenant tenant);
}