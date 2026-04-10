using AridentIam.Domain.Entities.Delegations;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IDelegationGrantRepository
{
    Task<DelegationGrant?> GetByExternalIdAsync(Guid delegationGrantExternalId, CancellationToken cancellationToken = default);
    Task AddAsync(DelegationGrant delegationGrant, CancellationToken cancellationToken = default);
    void Update(DelegationGrant delegationGrant);
}