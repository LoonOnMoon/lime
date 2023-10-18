using System.Reflection;

using Lime.Application.Extensions;
using Lime.Infrastructure.Identity.Extensions;
using Lime.Infrastructure.Jwt.Extensions;
using Lime.Infrastructure.Persistence.Extensions;
using Lime.Web.Middleware;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Mvc.Versioning;

using Serilog;

namespace Lime.Web.Extensions;

public static class WebStartupExtensions
{
    public static IServiceCollection AddWeb(this IServiceCollection services, IHostEnvironment env)
    {
        services.AddTransient<GlobalExceptionHandlingMiddleware>();

        // Add Asp to DI
        services.AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddApiVersioning(opt =>
                {
                    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                    opt.AssumeDefaultVersionWhenUnspecified = true;
                    opt.ReportApiVersions = true;
                    opt.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("x-api-version"),
                        new MediaTypeApiVersionReader("x-api-version"));
                })
            .AddControllers();

        // Mapster config setup
        var mapsterConfig = TypeAdapterConfig.GlobalSettings;
        mapsterConfig.Scan(Assembly.GetExecutingAssembly());

        // Add Infrastructure layer to DI
        services.AddDatabaseConfiguration(env)
            .AddDatabase(env)
            .AddJwtConfiguration()
            .AddIdentity(env, mapsterConfig);

        // Add Application layer to DI
        services.AddApplication(mapsterConfig);

        // Add Mapster
        services.AddSingleton(mapsterConfig);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }

    public static IApplicationBuilder UseWeb(this IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseSerilogRequestLogging();
        app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        app.UseRouting();

        // Configure the HTTP request pipeline.
        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

        // Call Web layer middleware registration
        app.UseIdentity();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.UseHttpsRedirection();

        return app;
    }
}