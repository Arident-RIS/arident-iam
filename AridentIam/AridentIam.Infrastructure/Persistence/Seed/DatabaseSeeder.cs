using AridentIam.Domain.Entities.Tenants;
using AridentIam.Domain.Enums;
using AridentIam.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AridentIam.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static readonly Guid DefaultTenantExternalId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    private const string SystemActor = "system";

    public static async Task SeedAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var logger = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseSeeder");

        var dbContext = scope.ServiceProvider.GetRequiredService<AridentIamDbContext>();

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            throw new InvalidOperationException(
                "Database seeding cannot start because there are still pending EF Core migrations.");
        }

        try
        {
            var tenantExists = await dbContext.Tenants.AnyAsync(
                x => x.TenantExternalId == DefaultTenantExternalId);

            if (tenantExists)
            {
                logger.LogInformation(
                    "Default tenant already exists with ExternalId {TenantExternalId}.",
                    DefaultTenantExternalId);

                return;
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                "Database seeding failed because the 'Tenants' table is not available. Ensure the initial EF Core migration has been created and applied successfully.",
                ex);
        }

        var tenant = Tenant.Create(
            tenantExternalId: DefaultTenantExternalId,
            code: "DEFAULT",
            name: "Default Tenant",
            isolationMode: IsolationMode.Shared,
            createdBy: SystemActor);

        tenant.SetLocale("en-US", SystemActor);
        tenant.SetTimeZone("UTC", SystemActor);

        await dbContext.Tenants.AddAsync(tenant);
        await dbContext.SaveChangesAsync();

        logger.LogInformation(
            "Default tenant seeded successfully with ExternalId {TenantExternalId}.",
            DefaultTenantExternalId);
    }
}