using AridentIam.Domain.Entities.Users;

namespace AridentIam.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByExternalIdAsync(Guid userExternalId, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailAsync(Guid tenantExternalId, string email, CancellationToken cancellationToken = default);

    Task<User?> GetByUsernameAsync(Guid tenantExternalId, string username, CancellationToken cancellationToken = default);

    Task<bool> ExistsByEmailAsync(Guid tenantExternalId, string email, CancellationToken cancellationToken = default);

    Task<bool> ExistsByUsernameAsync(Guid tenantExternalId, string username, CancellationToken cancellationToken = default);

    Task AddAsync(User user, CancellationToken cancellationToken = default);

    void Update(User user);
}