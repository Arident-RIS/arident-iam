using AridentIam.Domain.Entities.Users;
using AridentIam.Domain.Interfaces.Repositories;
using AridentIam.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AridentIam.Infrastructure.Persistence.Repositories.Users;

public sealed class UserRepository(AridentIamDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByExternalIdAsync(
        Guid userExternalId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.UserExternalId == userExternalId,
                cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(
        Guid tenantExternalId,
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();

        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.Email == normalizedEmail,
                cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(
        Guid tenantExternalId,
        string username,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim();

        return await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.Username == normalizedUsername,
                cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(
        Guid tenantExternalId,
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();

        return await dbContext.Users
            .AsNoTracking()
            .AnyAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.Email == normalizedEmail,
                cancellationToken);
    }

    public async Task<bool> ExistsByUsernameAsync(
        Guid tenantExternalId,
        string username,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim();

        return await dbContext.Users
            .AsNoTracking()
            .AnyAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.Username == normalizedUsername,
                cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);

        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public void Update(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        dbContext.Users.Update(user);
    }
}