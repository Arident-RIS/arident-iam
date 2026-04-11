using AridentIam.Infrastructure.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AridentIam.Infrastructure.Persistence.Context;

public sealed class AridentIamDbContextFactory : IDesignTimeDbContextFactory<AridentIamDbContext>
{
    public AridentIamDbContext CreateDbContext(string[] args)
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var possibleBasePaths = new[]
        {
            Directory.GetCurrentDirectory(),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "AridentIam.WebApi"),
            Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "AridentIam.WebApi")
        };

        var basePath = possibleBasePaths
            .Select(Path.GetFullPath)
            .FirstOrDefault(Directory.Exists);

        if (string.IsNullOrWhiteSpace(basePath))
        {
            throw new InvalidOperationException(
                "Unable to determine the base path for locating WebApi configuration files.");
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var databaseSettings = configuration
            .GetSection(DatabaseSettings.SectionName)
            .Get<DatabaseSettings>()
            ?? throw new InvalidOperationException(
                $"Configuration section '{DatabaseSettings.SectionName}' is missing for design-time DbContext creation.");

        var connectionString = DatabaseConnectionStringFactory.Build(databaseSettings);

        var optionsBuilder = new DbContextOptionsBuilder<AridentIamDbContext>();
        optionsBuilder.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly(typeof(AridentIamDbContextFactory).Assembly.FullName);
            sqlOptions.CommandTimeout(databaseSettings.CommandTimeoutSeconds);
        });

        return new AridentIamDbContext(optionsBuilder.Options);
    }
}