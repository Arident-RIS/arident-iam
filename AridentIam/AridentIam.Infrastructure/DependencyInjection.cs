using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AridentIam.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddHealthChecks()
            .AddSqlServer(
                connectionString,
                name: "sqlserver",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["database", "sqlserver"]);

        return services;
    }
}
