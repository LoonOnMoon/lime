using Lime.Web.Extensions;

using Serilog;

// Create logger for failures before DI
var appSettings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(appSettings)
    .CreateBootstrapLogger();

try
{
    Log.Information("Lime starting up...");

    var builder = WebApplication.CreateBuilder(args);

    var services = builder.Services;

    builder.Host
        .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

    // Call Web layer startup extension
    services.AddWeb(builder.Environment);

    var app = builder.Build();

    app.UseWeb(app.Environment);

    app.MapControllers();

    app.Run();
}
catch (HostAbortedException)
{
    Log.Information("Lime aborted by host...");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Lime failed to start...");
}
finally
{
    Log.CloseAndFlush();
}