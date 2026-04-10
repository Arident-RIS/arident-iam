using AridentIam.Domain.Entities.Roles;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IRoleDefinitionRepository : IRepository<RoleDefinition>
{
    Task<RoleDefinition?> GetByCodeAsync(Guid tenantExternalId, string code, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCodeAsync(Guid tenantExternalId, string code, CancellationToken cancellationToken = default);
}
