using AridentIam.Application.Common.Interfaces;
using AridentIam.Domain.Interfaces.Repositories;
using AridentIam.Infrastructure.Configuration;
using AridentIam.Infrastructure.Persistence.Context;
using AridentIam.Infrastructure.Persistence.Repositories.Organizations;
using AridentIam.Infrastructure.Persistence.Repositories.Principals;
using AridentIam.Infrastructure.Persistence.Repositories.Tenants;
using AridentIam.Infrastructure.Persistence.Repositories.Users;
using AridentIam.Infrastructure.Persistence.UnitOfWork;
using AridentIam.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AridentIam.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(
            configuration.GetSection(DatabaseSettings.SectionName));

        var databaseSettings = configuration
            .GetSection(DatabaseSettings.SectionName)
            .Get<DatabaseSettings>()
            ?? throw new InvalidOperationException(
                $"Configuration section '{DatabaseSettings.SectionName}' is missing.");

        var connectionString = DatabaseConnectionStringFactory.Build(databaseSettings);

        services.AddDbContext<AridentIamDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName);
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                sqlOptions.CommandTimeout(databaseSettings.CommandTimeoutSeconds);
            });
        });

        services.AddScoped<IPrincipalRepository, PrincipalRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();

        return services;
    }
}