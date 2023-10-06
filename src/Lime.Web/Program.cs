using Lime.Web.StartupExtensions;
using Lime.Web.StartupExtensions.ConfigurationExtensions;

using Microsoft.AspNetCore.Mvc.Versioning;

using Serilog;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.File("./Logs/log-.log", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateBootstrapLogger();

builder.Host
    .UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

try
{
    // Add configurations
    services.AddDatabaseConfiguration();
    services.AddJwtConfiguration();

    // Add services to the container.
    services.AddDatabase(configuration, builder.Environment);
    services.AddAuth(configuration);
    services.AddControllers();

    // services.AddApiVersioning(opt =>
    //     {
    //         opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    //         opt.AssumeDefaultVersionWhenUnspecified = true;
    //         opt.ReportApiVersions = true;
    //         opt.ApiVersionReader = ApiVersionReader.Combine(
    //             new UrlSegmentApiVersionReader(),
    //             new HeaderApiVersionReader("x-api-version"),
    //             new MediaTypeApiVersionReader("x-api-version"));
    //     });
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    var app = builder.Build();

    app.UseRouting();

    // Configure the HTTP request pipeline.
    app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    app.UseAuth();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application failed to start...");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}