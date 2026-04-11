using AridentIam.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AridentIam.Infrastructure.Persistence.Migrations;

public static class DatabaseMigrationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>()
            .CreateLogger("DatabaseMigration");

        var dbContext = scope.ServiceProvider.GetRequiredService<AridentIamDbContext>();

        logger.LogInformation("Applying database migrations for AridentIAM.");

        await dbContext.Database.MigrateAsync();

        var canConnect = await dbContext.Database.CanConnectAsync();
        if (!canConnect)
        {
            throw new InvalidOperationException(
                "Database migrations completed, but the application could not verify connectivity to the database.");
        }

        logger.LogInformation("Database migrations applied successfully.");
    }
}