using System.Reflection;

using Mapster;

using MapsterMapper;

namespace Lime.Web.Common.Mapping;

public static class MappingStartupExtension
{
    public static IServiceCollection AddMappings(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}