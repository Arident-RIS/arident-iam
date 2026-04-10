using AridentIam.Domain.Entities.Organizations;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IOrgUnitRepository : IRepository<OrgUnit>
{
    Task<IReadOnlyList<OrgUnit>> GetChildrenAsync(Guid parentOrgUnitExternalId, CancellationToken cancellationToken = default);
}
