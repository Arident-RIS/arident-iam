using AridentIam.Domain.Entities.Principals;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IPrincipalRepository : IRepository<Principal>
{
    Task<Principal?> GetByPrincipalExternalIdAsync(Guid principalExternalId, CancellationToken cancellationToken = default);
    Task<Principal?> GetHumanByEmailAsync(Guid tenantExternalId, string email, CancellationToken cancellationToken = default);
    Task<Principal?> GetHumanByUsernameAsync(Guid tenantExternalId, string username, CancellationToken cancellationToken = default);
    Task<bool> ExistsHumanByEmailAsync(Guid tenantExternalId, string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsHumanByUsernameAsync(Guid tenantExternalId, string username, CancellationToken cancellationToken = default);
}