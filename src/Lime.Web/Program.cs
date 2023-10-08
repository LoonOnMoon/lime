using Lime.Infrastructure.IoC;
using Lime.Web.Extensions.StartupExtensions;
using Lime.Web.Middleware;

using Serilog;

var appSettings = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(appSettings)
    .CreateBootstrapLogger();

try
{
    Log.Information("Lime starting up.");

    var builder = WebApplication.CreateBuilder(args);

    var services = builder.Services;

    builder.Host
        .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

    services.AddWeb()
        .RegisterServices(builder.Environment);

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
    app.UseRouting();

    // Configure the HTTP request pipeline.
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    // Call IoC middleware registration
    app.RegisterMiddleware();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (HostAbortedException)
{
    Log.Information("Lime aborted by host.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "Lime failed to start...");
}
finally
{
    Log.CloseAndFlush();
}