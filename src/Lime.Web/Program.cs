using Lime.Web.Middleware;
using Lime.Web.StartupExtensions;
using Lime.Web.StartupExtensions.ConfigurationExtensions;

using Microsoft.AspNetCore.Mvc.Versioning;

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
    var configuration = builder.Configuration;

    builder.Host
        .UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

    builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

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

    app.UseSerilogRequestLogging();

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

    app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

    // app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Lime failed to start...");
}
finally
{
    Log.CloseAndFlush();
}