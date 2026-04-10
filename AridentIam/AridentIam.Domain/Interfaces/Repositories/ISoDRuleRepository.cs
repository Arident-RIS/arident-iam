using AridentIam.Domain.Entities.Governance;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface ISoDRuleRepository
{
    Task<SoDRule?> GetByExternalIdAsync(Guid soDRuleExternalId, CancellationToken cancellationToken = default);
    Task<SoDRule?> GetByCodeAsync(Guid tenantExternalId, string code, CancellationToken cancellationToken = default);
    Task AddAsync(SoDRule soDRule, CancellationToken cancellationToken = default);
    void Update(SoDRule soDRule);
}