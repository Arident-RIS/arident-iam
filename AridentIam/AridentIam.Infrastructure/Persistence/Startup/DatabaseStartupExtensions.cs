using AridentIam.Infrastructure.Persistence.Migrations;
using AridentIam.Infrastructure.Persistence.Seed;

namespace AridentIam.Infrastructure.Persistence.Startup;

public static class DatabaseStartupExtensions
{
    public static async Task InitializeDatabaseAsync(this IServiceProvider services)
    {
        await services.ApplyDatabaseMigrationsAsync();
        await services.SeedAsync();
    }
}