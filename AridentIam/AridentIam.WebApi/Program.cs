using AridentIam.Application;
using AridentIam.Infrastructure;
using AridentIam.Infrastructure.Persistence.Startup;
using AridentIam.WebApi.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddWebApiServices(builder.Configuration);

    var app = builder.Build();

    await app.Services.InitializeDatabaseAsync();

    app.UseWebApiPipeline();

    Log.Information("AridentIAM API starting up");

    await app.RunAsync();
}
catch (Microsoft.Extensions.Hosting.HostAbortedException)
{
    // This can happen during EF Core design-time operations such as migrations.
}
catch (Exception ex)
{
    Log.Fatal(ex, "AridentIAM API terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}