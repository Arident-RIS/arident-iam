using AridentIam.Domain.Entities.Permissions;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IPermissionDefinitionRepository : IRepository<PermissionDefinition>
{
    Task<IReadOnlyList<PermissionDefinition>> GetByRoleAsync(Guid roleDefinitionExternalId, CancellationToken cancellationToken = default);
}
