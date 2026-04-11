using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AridentIam.Infrastructure.Persistence.Context;

public sealed class AridentIamDbContextFactory : IDesignTimeDbContextFactory<AridentIamDbContext>
{
    public AridentIamDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string 'DefaultConnection' was not found for design-time DbContext creation.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<AridentIamDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AridentIamDbContext(optionsBuilder.Options);
    }
}