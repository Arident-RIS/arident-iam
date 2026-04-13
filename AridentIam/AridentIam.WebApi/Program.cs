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

    _ = builder.Host.UseSerilog((context, loggerConfiguration) =>
        loggerConfiguration.ReadFrom.Configuration(context.Configuration));

    _ = builder.Services.AddApplication();
    _ = builder.Services.AddInfrastructure(builder.Configuration);
    _ = builder.Services.AddWebApiServices(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        await app.Services.InitializeDatabaseAsync();
    }

    _ = app.UseWebApiPipeline();

    Log.Information("AridentIAM API starting up");

    await app.RunAsync();
}
catch (HostAbortedException)
{
    // Expected during some EF Core design-time operations such as migrations.
}
catch (Exception ex)
{
    Log.Fatal(ex, "AridentIAM API terminated unexpectedly");
}
finally
{
    await Log.CloseAndFlushAsync();
}

public partial class Program
{
    protected Program() { }
}