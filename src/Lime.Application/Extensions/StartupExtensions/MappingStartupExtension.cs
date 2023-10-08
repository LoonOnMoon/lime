using System.Reflection;

using Mapster;
using MapsterMapper;

using Microsoft.Extensions.DependencyInjection;

namespace Lime.Application.Extensions.StartupExtensions;

public static class MappingStartupExtension
{
    public static IServiceCollection AddMappings(this IServiceCollection services, TypeAdapterConfig config)
    {
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config)
            .AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}