using Mapster;

using Microsoft.Extensions.DependencyInjection;

namespace Lime.Application.Extensions.StartupExtensions;

public static class ApplicationStartupExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services, TypeAdapterConfig typeAdapterConfig)
    {
        services.AddMappings(typeAdapterConfig)
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationStartupExtension).Assembly));
        return services;
    }
}