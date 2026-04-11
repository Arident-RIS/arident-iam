using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AridentIam.WebApi.IntegrationTests.Infrastructure;

/// <summary>
/// Shared factory for all integration tests.
/// Sets the environment to "Integration" so that:
///   - Swagger is enabled (ApplicationBuilderExtensions checks IsDevelopment || IsEnvironment("Integration"))
///   - Database auto-migration/seeding is skipped (Program.cs calls InitializeDatabaseAsync only in Development)
/// </summary>
public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Integration");
    }
}
