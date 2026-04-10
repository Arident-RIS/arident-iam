using AridentIam.Domain.Entities.Delegations;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IDelegationDefinitionRepository
{
    Task<DelegationDefinition?> GetByExternalIdAsync(Guid delegationDefinitionExternalId, CancellationToken cancellationToken = default);
    Task<DelegationDefinition?> GetByCodeAsync(Guid tenantExternalId, string code, CancellationToken cancellationToken = default);
    Task AddAsync(DelegationDefinition delegationDefinition, CancellationToken cancellationToken = default);
    void Update(DelegationDefinition delegationDefinition);
}