using Lime.Infrastructure.Identity;
using Lime.Infrastructure.Jwt;
using Lime.Infrastructure.Persistence;

using Microsoft.AspNetCore.Builder;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lime.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IHostEnvironment env, TypeAdapterConfig typeAdapterConfig)
    {
        // Add Infrastructure layer to DI
        services.AddDatabaseConfiguration(env)
            .AddPersistence(env)
            .AddJwtConfiguration()
            .AddIdentity(env, typeAdapterConfig);
        return services;
    }

    public static IApplicationBuilder RegisterInfrastructureMiddleware(this IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseIdentity();

        return app;
    }
}