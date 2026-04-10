using AridentIam.Domain.Entities.Groups;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IGroupRepository : IRepository<Group>
{
    Task<IReadOnlyList<Group>> GetByTenantAsync(Guid tenantExternalId, CancellationToken cancellationToken = default);
}
