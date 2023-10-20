using Lime.Web;

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
    services.AddWebServices(builder.Environment);

    var app = builder.Build();

    app.RegisterWebMiddleware(app.Environment);

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