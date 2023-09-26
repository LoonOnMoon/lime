using Microsoft.Extensions.DependencyInjection;

namespace Lime.Infrastructure.IoC;

public class InjectionBootStrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }
}