using AridentIam.Domain.Entities.Tenants;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface ITenantRepository : IRepository<Tenant>
{
    Task<Tenant?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}
