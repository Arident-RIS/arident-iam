using AridentIam.Domain.Entities.Principals;
using AridentIam.Domain.Enums;
using AridentIam.Domain.Interfaces.Repositories;
using AridentIam.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace AridentIam.Infrastructure.Persistence.Repositories.Principals;

public sealed class PrincipalRepository(AridentIamDbContext dbContext) : IPrincipalRepository
{
    public async Task AddAsync(Principal entity, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity);

        await dbContext.Principals.AddAsync(entity, cancellationToken);
    }

    public async Task<Principal?> GetByExternalIdAsync(
        Guid externalId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Principals
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(
                x => x.PrincipalExternalId == externalId,
                cancellationToken);
    }

    public async Task<Principal?> GetByPrincipalExternalIdAsync(
        Guid principalExternalId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.Principals
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(
                x => x.PrincipalExternalId == principalExternalId,
                cancellationToken);
    }

    public async Task<Principal?> GetHumanByEmailAsync(
        Guid tenantExternalId,
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();

        return await dbContext.Principals
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.PrincipalType == PrincipalType.User &&
                     x.UserProfile != null &&
                     x.UserProfile.Email == normalizedEmail,
                cancellationToken);
    }

    public async Task<Principal?> GetHumanByUsernameAsync(
        Guid tenantExternalId,
        string username,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim();

        return await dbContext.Principals
            .Include(x => x.UserProfile)
            .FirstOrDefaultAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.PrincipalType == PrincipalType.User &&
                     x.UserProfile != null &&
                     x.UserProfile.Username == normalizedUsername,
                cancellationToken);
    }

    public async Task<bool> ExistsHumanByEmailAsync(
        Guid tenantExternalId,
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();

        return await dbContext.Principals
            .Include(x => x.UserProfile)
            .AnyAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.PrincipalType == PrincipalType.User &&
                     x.UserProfile != null &&
                     x.UserProfile.Email == normalizedEmail,
                cancellationToken);
    }

    public async Task<bool> ExistsHumanByUsernameAsync(
        Guid tenantExternalId,
        string username,
        CancellationToken cancellationToken = default)
    {
        var normalizedUsername = username.Trim();

        return await dbContext.Principals
            .Include(x => x.UserProfile)
            .AnyAsync(
                x => x.TenantExternalId == tenantExternalId &&
                     x.PrincipalType == PrincipalType.User &&
                     x.UserProfile != null &&
                     x.UserProfile.Username == normalizedUsername,
                cancellationToken);
    }

    public void Update(Principal entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        dbContext.Principals.Update(entity);
    }

    public void Remove(Principal entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        dbContext.Principals.Remove(entity);
    }
}