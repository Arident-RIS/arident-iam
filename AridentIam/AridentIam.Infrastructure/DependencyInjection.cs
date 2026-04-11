using AridentIam.Domain.Interfaces.Repositories;
using AridentIam.Infrastructure.Persistence.Context;
using AridentIam.Infrastructure.Persistence.Repositories.Principals;
using AridentIam.Infrastructure.Persistence.Repositories.Tenants;
using AridentIam.Infrastructure.Persistence.Repositories.Users;
using AridentIam.Infrastructure.Persistence.UnitOfWork;
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
            });
        });

        services.AddScoped<IPrincipalRepository, PrincipalRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}