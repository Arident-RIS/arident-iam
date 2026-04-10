using AridentIam.Domain.Entities.Policies;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IPolicyDefinitionRepository : IRepository<PolicyDefinition>
{
    Task<PolicyDefinition?> GetByCodeAsync(Guid tenantExternalId, string code, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PolicyDefinition>> GetActivePoliciesAsync(Guid tenantExternalId, CancellationToken cancellationToken = default);
}
