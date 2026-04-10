using AridentIam.Domain.Entities.Integrations;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IExternalSystemRepository
{
    Task<ExternalSystem?> GetByExternalIdAsync(Guid externalSystemExternalId, CancellationToken cancellationToken = default);
    Task AddAsync(ExternalSystem externalSystem, CancellationToken cancellationToken = default);
    void Update(ExternalSystem externalSystem);
}