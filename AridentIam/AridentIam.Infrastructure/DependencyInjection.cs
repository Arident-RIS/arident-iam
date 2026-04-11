using AridentIam.Application.Common.Interfaces;
using AridentIam.Domain.Interfaces.Repositories;
using AridentIam.Infrastructure.Persistence.Context;
using AridentIam.Infrastructure.Persistence.Repositories.Principals;
using AridentIam.Infrastructure.Persistence.Repositories.Tenants;
using AridentIam.Infrastructure.Persistence.Repositories.Users;
using AridentIam.Infrastructure.Persistence.UnitOfWork;
using AridentIam.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AridentIam.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string 'DefaultConnection' was not found.");
        }

        services.AddDbContext<AridentIamDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
        });

        services.AddScoped<IPrincipalRepository, PrincipalRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHealthChecks()
            .AddSqlServer(
                connectionString,
                name: "sqlserver",
                failureStatus: HealthStatus.Unhealthy,
                tags: ["database", "sqlserver"]);

        return services;
    }
}